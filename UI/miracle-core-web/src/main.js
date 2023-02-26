import Vue from "vue";
import App from "./App.vue";
import VueRouter from "vue-router";
import Axios from "axios";
import VueAxios from "vue-axios";
import { router } from "./router";
import vuetify from "./plugins/vuetify";
import CKEditor from "ckeditor4-vue";
import { store } from "./store";

Vue.use(CKEditor);
Vue.use(VueAxios, Axios);
Vue.use(VueRouter);

router.beforeEach((to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    let webToken = localStorage.getItem("webToken");
    let username = localStorage.getItem("username");
    if (webToken === null || username === null) {
      next({
        path: "account/login",
        query: { redirect: to.fullPath },
      });
    } else {
      next();
    }
  } else if (to.path.includes("forgot-password-res")) {
    next();
  } else {
    next();
  }
});
router.beforeResolve((to, from, next) => {
  next(); // DO IT!
});

Vue.mixin({
  methods: {
    translator(key) {
      return this.$vuetify.lang.t("$vuetify." + key);
    },
  },
});

new Vue({
  router,
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
