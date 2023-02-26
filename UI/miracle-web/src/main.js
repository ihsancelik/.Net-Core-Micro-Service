import Vue from "vue";
import App from "./App.vue";
import VueRouter from "vue-router";
import Axios from "axios";
import VueAxios from "vue-axios";
import { router } from "./router";
import vuetify from "./plugins/vuetify";
import VuePreviewer from "vue-image-previewer";
import CKEditor from "ckeditor4-vue";
import { store } from "./store/store";
import * as VueSignalR from "@aspnet/signalr";
import 'fullpage.js/vendors/scrolloverflow' // Optional. When using scrollOverflow:true
import VueFullPage from 'vue-fullpage.js'

Vue.use(VueFullPage);

Vue.use(VueAxios, Axios);
Vue.use(VueRouter);
Vue.use(VuePreviewer);
Vue.use(CKEditor);
Vue.use(VueSignalR, "");

router.beforeEach((to, from, next) => {
  let webToken = localStorage.getItem("webToken");
  let username = localStorage.getItem("username");
  let roles = localStorage.getItem("roles");
  roles = roles !== null ? roles.split(",") : null;

  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (webToken === "" || username === "" || roles === null) {
      next({
        path: "/account/login",
        query: to.query
      });

    } else if (to.matched.some((record) => record.meta.requiresAdmin)) {
      let isAdmin = false;
      roles.forEach((role) => {
        if (role === "Admin" || role === "SoftwareDeveloper") isAdmin = true;
      });
      if (!isAdmin) next("/user");
      else next();
    } else next();
  } else if (to.path.includes("forgot-password-res")) {
    next();
  } else {
    next();
  }
});

let ui = Date.now();
if (localStorage.getItem("room") == null) {
  localStorage.setItem("room", ui);
}

Vue.mixin({
  methods: {
    translator(key) {
      return this.$vuetify.lang.t("$vuetify." + key);
    },
  },
});

Vue.filter("currency", (value) => {
  return parseFloat(value).toLocaleString(undefined, { minimumFractionDigits: 2 }) + " TRY";
});

new Vue({
  router,
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
