import axios from "axios";
import { AccountRoutes, AuthenticateRoutes } from "@/helpers/RouteConstraints";
import { router } from "@/router";
import { ErrorConstraints } from "@/helpers/Constraints";

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
    this.removeLocalStorage();
    if (
      webToken !== undefined &&
      webToken !== null &&
      webRefreshToken !== undefined &&
      webRefreshToken !== null &&
      username !== undefined &&
      username !== null
    ) {
      this.addLocalStorage(webToken, webRefreshToken, username);
    }
    if (response.success) {
      if (router.currentRoute.query.redirect !== undefined) router.push(router.currentRoute.query.redirect).catch();
      else router.push("/").catch();
    } else return response;
  }

  async loginByRefreshToken() {
    let response = await this.request(
      "POST",
      AuthenticateRoutes.AuthenticateByRefreshToken(),
      {
        "Content-Type": "application/json",
        "Authentication-Type": "Web",
      },
      { refreshToken: localStorage.getItem("webRefreshToken") }
    );
    let webToken = response.webToken;
    let webRefreshToken = response.webRefreshToken;
    let username = response.username;

    this.removeLocalStorage();

    if (
      webToken !== undefined &&
      webToken !== null &&
      webRefreshToken !== undefined &&
      webRefreshToken !== null &&
      username !== undefined &&
      username !== null
    ) {
      this.addLocalStorage(webToken, webRefreshToken, username);
    }
    if (response.success) {
      if (router.currentRoute.query.redirect !== undefined) router.push(router.currentRoute.query.redirect).catch();
      return response.success;
    } else {
      router.push({ path: "/account/login", query: { redirect: router.currentRoute.fullPath } });
      throw ErrorConstraints.AUTHENTICATE_FAILED;
    }
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

  addLocalStorage(webToken, webRefreshToken, username) {
    localStorage.setItem("webToken", webToken);
    localStorage.setItem("webRefreshToken", webRefreshToken);
    localStorage.setItem("username", username);
  }
  removeLocalStorage() {
    localStorage.removeItem("webToken");
    localStorage.removeItem("webRefreshToken");
    localStorage.removeItem("username");
  }
}
