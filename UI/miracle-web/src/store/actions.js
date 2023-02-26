/* eslint-disable */

import { MarketRoutes } from "@/helpers/RouteConstraints";
import BaseService from "@/services/BaseService";

// API'den kurların karşılığını aldım
export const getCurrency = async ({}, codes) => {
  let response = await new BaseService("").request("POST", MarketRoutes.GetCurrency(), codes);
  return response.list;
};

export const initApp = async ({ dispatch, state }) => {
  await dispatch("getCurrency", state.currencyList)
  .then((response) => {
    response.forEach((element) => {
      state.currencies.push({ currency: element.currency, value: element.value });
    });
  });
};

export const readyToPurchase = async () => {
  let response = await new BaseService("").request("GET", MarketRoutes.ReadyToPurchase());
  return response.success;
};

export const purchase = async ({}, payload) => {
  let response = await new BaseService("").request("POST", MarketRoutes.PurchaseOrder(), payload);
  return response.success;
};

export const addToCart = ({ commit }, payload) => {
  commit("addToCart", payload);
  commit("saveCart");
};

export const removeToCart = ({ commit }, payload) => {
  commit("removeToCart", payload);
  commit("saveCart");
};

export const decrementQuantity = ({ commit }, payload) => {
  commit("decrementQuantity", payload);
  commit("saveCart");
};

export const clearCart = ({ commit }) => {
  commit("clearCart");
};