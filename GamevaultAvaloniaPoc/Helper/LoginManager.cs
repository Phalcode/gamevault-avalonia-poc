﻿using GamevaultAvaloniaPoc.Models;
using GamevaultAvaloniaPoc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GamevaultAvaloniaPoc.Helper
{
	public enum LoginState
	{
		Success,
		Error,
		Unauthorized,
		Forbidden
	}
	internal class LoginManager
	{
		#region Singleton
		private static LoginManager instance = null;
		private static readonly object padlock = new object();

		public static LoginManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new LoginManager();
					}
					return instance;
				}
			}
		}
		#endregion
		private User? m_User { get; set; }
		private LoginState m_LoginState { get; set; }
		public User? GetCurrentUser()
		{
			return m_User;
		}
		public bool IsLoggedIn()
		{
			return m_User != null;
		}
		public LoginState GetState()
		{
			return m_LoginState;
		}
		public void SwitchToOfflineMode()
		{
			m_User = null;
		}

		public async Task<LoginState> ManualLogin(string username, string password)
		{
			LoginState state = LoginState.Success;
			User? user = await Task<User>.Run(() =>
			{
				try
				{					
					string result = WebHelper.GetRequest(@$"{SettingsViewModel.Instance.ServerUrl}api/v1/users/me");
					return JsonSerializer.Deserialize<User>(result);
				}
				catch (WebException ex)
				{
					string code = WebExceptionHelper.GetServerStatusCode(ex);					
					state = DetermineLoginState(code);
					return null;
				}
				catch
				{
					state = LoginState.Error;
					return null;
				}
			});
			m_User = user;
			m_LoginState = state;
			MainViewModel.Instance.UserIcon = m_User;
			return state;
		}
		private LoginState DetermineLoginState(string code)
		{
			switch (code)
			{
				case "401":
					{
						return LoginState.Unauthorized;
					}
				case "403":
					{
						return LoginState.Forbidden;
					}
			}
			return LoginState.Error;
		}
	}
}
