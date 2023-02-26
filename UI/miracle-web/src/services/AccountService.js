import { AccountRoutes, AuthenticateRoutes } from "@/helpers/RouteConstraints";
import { router } from "@/router";

import axios from "axios";

export default class AccountService {
  async request(method, url, headers, data = {}) {
    try {
      let result = await axios({
        method: method,
        url: url,
        headers: headers,
        data: data,
      });
      return result.data;
    } catch (e) {
      throw e;
    }
  }

  async registerUser(model) {
    return this.request("POST", AccountRoutes.Register(), { "Content-Type": "application/json" }, model);
  }

  async login(model) {
    let response = await this.request(
      "POST",
      AuthenticateRoutes.Authenticate(),
      { "Content-Type": "application/json", "Authentication-Type": "Web" },
      model
    );

    let webToken = response.webToken;
    let webRefreshToken = response.webRefreshToken;
    let username = response.username;
    let roles = response.roles;

    this.removeLocalStorage();

    if (webToken !== undefined && webRefreshToken !== undefined && username !== undefined && roles !== undefined) {
      this.addLocalStorage(webToken, webRefreshToken, username, roles);
    }
    if (response.success) {
      if (roles.includes("Admin") || (roles.includes("SoftwareDeveloper"))) {
        let route = router.currentRoute.query.redirect === undefined ? "/admin" : router.currentRoute.query.redirect;
        router.push(route);
      } else {
        let route = router.currentRoute.query.redirect === undefined ? "/user" : router.currentRoute.query.redirect;
        router.push(route);
      }
    } else return response;
  }

  async loginByRefreshToken() {
    let response = await this.request(
      "POST",
      AuthenticateRoutes.AuthenticateByRefreshToken(),
      { "Content-Type": "application/json", "Authentication-Type": "Web" },
      { WebRefreshToken: localStorage.getItem("webRefreshToken") }
    );
    let webToken = response.webToken;
    let webRefreshToken = response.webRefreshToken;
    let username = response.username;
    let roles = response.roles;

    this.removeLocalStorage();

    if (webToken !== undefined && webRefreshToken !== undefined && username !== undefined && roles !== undefined) {
      this.addLocalStorage(webToken, webRefreshToken, username, roles);
    }
    if (response.success) {
      if (roles.includes("Admin")) {
        let route = router.currentRoute.query.redirect === undefined ? "/admin" : router.currentRoute.query.redirect;
        router.push(route);
        return true;
      } else {
        let route = router.currentRoute.query.redirect === undefined ? "/user" : router.currentRoute.query.redirect;
        router.push(route);
        return true;
      }
    }
    return false;
  }

  async logout() {
    let response = await this.request("GET", AuthenticateRoutes.RevokeAuthenticate(), {
      "Content-Type": "application/json",
      Authorization: "Bearer " + localStorage.getItem("webToken"),
    });
    if (response.success) {
      this.removeLocalStorage();
      return response.success;
    }
  }

  async forgotPasswordRequest(model) {
    return await this.request("POST", AccountRoutes.ForgotPassword(), { "Content-Type": "application/json" }, model);
  }

  async forgotPasswordResponse(model) {
    this.removeLocalStorage();
    return await this.request("POST", AccountRoutes.ChangePassword(), { "Content-Type": "application/json" }, model);
  }

  async passwordReset(model) {
    return await this.request(
      "POST",
      AccountRoutes.ResetPasswordOutSource(),
      { "Content-Type": "application/json", Authorization: "Bearer " + localStorage.getItem("webToken") },
      model
    );
  }

  addLocalStorage(webToken, webRefreshToken, username, roles) {
    localStorage.setItem("webToken", webToken);
    localStorage.setItem("webRefreshToken", webRefreshToken);
    localStorage.setItem("username", username);
    localStorage.setItem("roles", roles);
  }
  removeLocalStorage() {
    localStorage.removeItem("webToken");
    localStorage.removeItem("webRefreshToken");
    localStorage.removeItem("username");
    localStorage.removeItem("roles");
  }
}
