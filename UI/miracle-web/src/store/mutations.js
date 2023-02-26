/* eslint-disable */
import Vue from "vue";

export const addToCart = (state, product) => {
  let found = state.cart.find((item) => item.id === product.id);
  if (found) {
    found.quantity++;
    found.totalPrice = found.quantity * found.price;
  } else {
    state.cart.push(product);

    Vue.set(product, "quantity", 1);
    Vue.set(product, "totalPrice", product.price);
  }
  state.cartCount++;
};

export const removeToCart = (state, product) => {
  let index = state.cart.indexOf(product);

  if (index > -1) {
    let product = state.cart[index];
    state.cartCount -= product.quantity;

    state.cart.splice(index, 1);
  }
};

export const decrementQuantity = (state, product) => {
  let index = state.cart.indexOf(product);

  if (index > -1) {
    let prod = state.cart[index];
    state.cartCount--;
    prod.quantity--;

    Vue.set(product, "totalPrice", prod.quantity * prod.price);

    if (prod.quantity === 0) state.cart.splice(index, 1);
  }
};

export const clearCart = (state) => {
  state.cart = [];
  state.cartCount = 0;
  localStorage.removeItem("cart");
  localStorage.removeItem("cartCount");
};

export const saveCart = (state) => {
  localStorage.setItem("cart", JSON.stringify(state.cart));
  localStorage.setItem("cartCount", state.cartCount);
};

export const setTicketGroupId = (state, ticketGroupId) => {
  state.ticketGroupId = ticketGroupId;
  return state.ticketGroupId;
}