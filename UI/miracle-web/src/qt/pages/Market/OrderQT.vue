<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="10">
        <v-card>
          <v-data-table :items-per-page="5" :headers="headers" :items="carts" item-key="key">
            <template #item.img="{ item }">
              <v-img :src="item.img" max-width="75px" />
            </template>
            <template #item.quantity="{ item }">
              <v-icon small @click="decrementQuantity(item)">mdi-minus-circle</v-icon>
              {{ item.quantity }}
              <v-icon small @click="incrementQuantity(item)">mdi-plus-circle</v-icon>
            </template>
            <template #item.unitTotal="{ item }"> {{ unitTotalPriceCounter(item) | currency }} </template>
            <template #item.actions="{ item }">
              <v-icon md class="mr-2" @click="removeItem(item)">mdi-delete</v-icon>
            </template>
          </v-data-table>
        </v-card>
      </v-col>
      <v-col
        class="mt-3 elevation-5"
        cols="12"
        md="2"
        style="border: 1px solid #7f7f7f; text-align: center; max-height: 200px;"
      >
        <h3 class="mb-3">{{ translator("subTotal").toUpperCase() }}</h3>
        <div class="ma-5">{{ translator("total") }} : {{ totalPriceCounter() | currency }}</div>
        <v-btn class="mt-3" outlined color="success" width="100%" @click="purchase">
          {{ translator("purchase") }}
        </v-btn>
      </v-col>
    </v-row>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-container>
</template>

<script>
import CheckOut from "./CheckOutQT";
import { CurrencyConsts } from "@/helpers/Constraints";
import { mapGetters } from "vuex";

export default {
  data() {
    return {
      headers: [
        { text: "#", value: "img" },
        { text: this.translator("product"), value: "name" },
        { text: this.translator("version"), value: "version" },
        { text: this.translator("price"), value: "price" },
        { text: this.translator("quantity"), value: "quantity" },
        { text: this.translator("currency"), value: "currency" },
        { text: this.translator("unitTotal") + " (" + CurrencyConsts.TL + ")", value: "unitTotal" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
    };
  },
  created() {
    this.initialize();
  },

  computed: mapGetters(["carts", "currencyList", "currencies"]),

  methods: {
    async initialize() {
      await this.$store.dispatch("initApp");
    },

    unitTotalPriceCounter(item) {
      let totalTL = 0;
      if (item.currency === CurrencyConsts.TL) totalTL += item.price * item.quantity;
      for (let i = 0; i < this.carts.length; i++) {
        if (item.currency === this.currencies[i].currency) {
          totalTL += item.totalPrice * this.currencies.find((curr) => curr.currency == item.currency).value;
          return totalTL.toFixed(2);
        }
      }
    },
    totalPriceCounter() {
      let totalTl = 0.0;
      this.carts.forEach((item) => {
        totalTl += parseFloat(this.unitTotalPriceCounter(item));
      });

      return totalTl.toFixed(2);
    },
    async purchase() {
      let response = await this.$store.dispatch("readyToPurchase");
      if (response) {
        this.selectedComponent = CheckOut;
        this.componentProps = { ordersProp: this.carts, totalPriceProp: this.totalPriceCounter() };

        this.openDialog(500);
      }
    },
    incrementQuantity(product) {
      this.$store.dispatch("addToCart", product);
    },
    decrementQuantity(product) {
      this.$store.dispatch("decrementQuantity", product);
    },
    removeItem(product) {
      this.$store.dispatch("removeToCart", product);
      //Sepet içinden sipariş silinirken nesne tanımama sorunu oluşuyor. Bu şekilde tüm siparişlerde id arayarak çözüldü.
      this.carts.forEach((order) => {
        if (order.id === product.id) this.carts.splice(this.carts.indexOf(order), 1);
      });
      this.initialize();
    },
    clearCart() {
      this.carts = [];

      this.$store.dispatch("clearCart");
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      if (changes) {
        this.clearCart();
        this.initialize();
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
};
</script>

<style scoped>
input[type="number"]::-webkit-inner-spin-button {
  display: none !important;
}
</style>
