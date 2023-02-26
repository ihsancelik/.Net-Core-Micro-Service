import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export const store = new Vuex.Store({
  state: {
    progress: {
      uploadProgressL: 0,
      uploadProgressM: 0,
      uploadProgressW: 0,
    },
  },
});
