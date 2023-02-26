import Vue from "vue";
import VueRouter from "vue-router";

//Layouts
import AuthenticatedLayout from "./layouts/AuthenticatedLayout";
import NotAuthLayout from "./layouts/NotAuthLayout";

//Pages
import Home from "./pages/Home/Home";
import CompanyIndex from "./pages/Company/CompanyIndex";
import NoticeIndex from "./pages/Notice/NoticeIndex";
import UserIndex from "./pages/User/UserIndex";
import RoleIndex from "./pages/Role/RoleIndex";
import PriorityIndex from "./pages/Priority/PriorityIndex";
import SMTPSettingIndex from "./pages/SMTPSetting/SMTPSettingIndex";
import LoggingIndex from "./pages/Logging/LoggingIndex";
import ProductIndex from "./pages/Product/ProductIndex";
import ProductModuleIndex from "./pages/ProductModule/ProductModuleIndex";
import ProductTagIndex from "./pages/ProductTag/ProductTagIndex";
import VersionInfoIndex from "./pages/VersionInfo/VersionInfoIndex";
import Server from "./pages/API/Server";

import Login from "./pages/Account/Login";
import ForgotPasswordReq from "./pages/Account/ForgotPasswordRequest";
import ForgotPasswordRes from "./pages/Account/ForgotPasswordResponse";

import ComingSoon from "./pages/ComingSoon";

import UserWatchIndex from "./pages/UserWatch/UserWatchIndex";
import DependencyIndex from "./pages/Dependency/DependencyIndex";
import ServerInfoIndex from "./pages/ServerInfo/ServerInfoIndex";

Vue.use(VueRouter);

export const routes = [
  {
    path: "/account",
    component: NotAuthLayout,
    children: [
      { path: "login", component: Login, name: "login", meta: { requiresAuth: false } },
      {
        path: "forgot-password-req",
        component: ForgotPasswordReq,
        name: "forgot-password-req",
        meta: { requiresAuth: false },
      },
      { path: "forgot-password-res/:code", component: ForgotPasswordRes, meta: { requiresAuth: false } },
      { path: "coming-soon", component: ComingSoon, meta: { requiresAuth: false } },
    ],
    meta: { requiresAuth: false },
  },

  {
    path: "/",
    component: AuthenticatedLayout,
    children: [
      { path: "/", component: Home, name: "home", meta: { requiresAuth: true } },
      { path: "users", component: UserIndex, name: "users", meta: { requiresAuth: true } },
      { path: "roles", component: RoleIndex, name: "roles", meta: { requiresAuth: true } },
      { path: "companies", component: CompanyIndex, name: "companies", meta: { requiresAuth: true } },
      { path: "priorities", component: PriorityIndex, name: "priorities", meta: { requiresAuth: true } },
      { path: "smtp-settings", component: SMTPSettingIndex, name: "smtp-settings", meta: { requiresAuth: true } },
      { path: "products", component: ProductIndex, name: "products", meta: { requiresAuth: true } },
      { path: "product-modules", component: ProductModuleIndex, name: "product-modules", meta: { requiresAuth: true } },
      { path: "notices", component: NoticeIndex, name: "notices", meta: { requiresAuth: true } },
      { path: "product-tags", component: ProductTagIndex, name: "product-tags", meta: { requiresAuth: true } },
      { path: "version-infos", component: VersionInfoIndex, name: "version-infos", meta: { requiresAuth: true } },
      { path: "server", component: Server, name: "server", meta: { requiresAuth: true } },
      { path: "logs", component: LoggingIndex, name: "logs", meta: { requiresAuth: true } },
      { path: "user-watch", component: UserWatchIndex, name: "user-watch", meta: { requiresAuth: true } },
      { path: "dependencies", component: DependencyIndex, name: "dependencies", meta: { requiresAuth: true } },
      { path: "server-info", component: ServerInfoIndex, name: "server-info", meta: { requiresAuth: true } },
    ],
    meta: { requiresAuth: true },
  },

  //PAGE ORIENTATION
  { path: "*", redirect: "/" },
];

export const router = new VueRouter({
  mode: "history",
  routes,
});
