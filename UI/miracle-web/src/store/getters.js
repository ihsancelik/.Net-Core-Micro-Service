/* eslint-disable */

export const getProducts = (state) => {
  return state.cart ? JSON.parse(state.cart) : [];
};

export const carts = (state) => {
  return state.cart;
  //return state.cart.length > 0 ? state.cart : localStorage.getItem("cart") !== null ? JSON.parse(localStorage.getItem("cart")) : [];
};

export const currencyList = (state) => {
  return state.currencyList;
};

export const currencies = (state) => {
  return state.currencies;
};

export const getTicketGroupId = (state) => {
  return state.ticketGroupId;
}