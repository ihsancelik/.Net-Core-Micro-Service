<template>
  <div class="text-center">
    <v-menu offset-y>
      <template #activator="{ on, attrs }">
        <a :attrs="attrs" v-on="on">
          <v-icon>mdi-cart-plus</v-icon>
          <span class="cardChip">
            {{ $store.state.cartCount }}
          </span>
        </a>
      </template>

      <v-card
        v-if="$store.state.cartCount > 0"
        class="text-center"
        style="width: 450px; max-height: 450px; overflow-y: scroll;"
      >
        <v-card-title style="font-size: medium;">{{ translator("yourCart") }}</v-card-title>

        <v-list v-model="carts">
          <v-list-item v-for="(item, index) in carts" :key="index">
            <v-card-text
              :style="index % 2 === 0 ? 'background-color:#efefef' : 'background-color:#ffffff'"
              style="height= 40%;"
            >
              <v-row>
                <v-col cols="12" md="4">
                  <v-img :src="item.img" max-width="70%" />
                </v-col>
                <v-col cols="12" md="4"> {{ item.name }} - {{ item.version }} </v-col>
                <v-col cols="12" md="4"> {{ item.price }} x {{ item.quantity }} - {{ item.totalPrice }} </v-col>
              </v-row>
            </v-card-text>
          </v-list-item>
        </v-list>
      </v-card>

      <v-btn v-else @click="market">
        <v-icon>mdi-cart</v-icon>
        {{ translator("emptyCart") }}
      </v-btn>

      <v-card v-if="$store.state.cartCount > 0">
        <v-card-actions>
          <span>
            <b> {{ translator("total") }}:</b> {{ totalPriceCount() | currency }}
          </span>
        </v-card-actions>
        <v-card-actions>
          <v-row>
            <v-btn color="primary darken-1" text @click="proceed">{{ translator("proceed") }}</v-btn>
            <v-spacer />
            <v-btn color="red darken-1" text @click="clearCart">{{ translator("clear") }}</v-btn>
          </v-row>
        </v-card-actions>
      </v-card>
    </v-menu>
  </div>
</template>

<script>
/* eslint-disable */
import { CurrencyConsts } from "@/helpers/Constraints";
import { mapGetters } from "vuex";

export default {
  async created() {
    await this.$store.dispatch("initApp");
  },
  computed: mapGetters(["carts", "currencyList", "currencies"]),

  methods: {
    // Sepet toplamı
    totalPriceCount() {
      let totalTL = 0.0;
      this.carts.forEach((item) => {
        if (item.currency === CurrencyConsts.TL) totalTL += item.price * item.quantity;

        if (item.currency === CurrencyConsts.USD)
          totalTL += item.totalPrice * this.currencies.find((curr) => curr.currency == CurrencyConsts.USD).value;

        if (item.currency === CurrencyConsts.EURO)
          totalTL += item.totalPrice * this.currencies.find((curr) => curr.currency == CurrencyConsts.EURO).value;

        if (item.currency === CurrencyConsts.POUND)
          totalTL += item.totalPrice * this.currencies.find((curr) => curr.currency == CurrencyConsts.POUND).value;
      });
      return totalTL.toFixed(2);
    },

    clearCart() {
      this.carts = [];
      this.$store.dispatch("clearCart");
    },
    proceed() {
      if (this.$store.state.cartCount > 0) this.$router.push("order");
    },
    market() {
      this.$router.push({
        path: "/market",
      });
    },
  },
};
</script>

<style>
.cardChip {
  border-radius: 9px;
  color: #fff;
  background-color: red;
  margin-top: -5px;
  margin-left: -5px;
  position: absolute;
  width: auto;
  text-align: center;
  padding: 0 4px 0 4px;
}
.empty {
  width: 175px;
  height: 100px;
  place-content: center;
  padding-top: 10%;
  margin-left: 3%;
  background-color: white;
}
</style>
