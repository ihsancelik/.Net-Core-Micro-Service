/* eslint-disable */

import Vue from "vue";
import Vuex from "vuex";
import * as getters from "./getters";
import * as mutations from "./mutations";
import * as actions from "./actions";
import live from "./modules/live";
import { CurrencyConsts } from "@/helpers/Constraints";

Vue.use(Vuex);
let cart = localStorage.getItem("cart");
let cartCount = localStorage.getItem("cartCount");

export const store = new Vuex.Store({
  state: {
    cart: cart ? JSON.parse(cart) : [],
    cartCount: cartCount ? parseInt(cartCount) : 0,
    currencyList: [CurrencyConsts.EURO, CurrencyConsts.USD, CurrencyConsts.POUND],
    currencies: [],
    ticketGroupId: 0
  },
  getters,
  mutations,
  actions,
  modules: {
    live,
  },
});
