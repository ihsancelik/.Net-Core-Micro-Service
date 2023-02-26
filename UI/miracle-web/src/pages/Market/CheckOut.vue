<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">CHECKOUT PROTOTYPE</h2>
      <hr class="mb-5" />

      <h1>TOTAL: {{ totalPrice }}</h1>
      <br />
      <br />
      <v-text-field outlined placeholder="FULLNAME"></v-text-field>
      <v-text-field outlined placeholder="CREDIT CARD NUMBER"></v-text-field>
      <v-text-field outlined placeholder="CREDIT CARD DATE"></v-text-field>
      <v-text-field outlined placeholder="CVC CODE"></v-text-field>
      <v-btn width="100%" height="80px" color="success" outlined @click="checkOut">PAY {{ totalPrice }} ₺</v-btn>
    </v-card>
  </ValidationObserver>
</template>

<script>
/* eslint-disable */
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  props: ["ordersProp", "totalPriceProp"],
  data() {
    return {
      orders: [],
      totalPrice: 0,
      signalModel: {
        changes: false,
        returnValues: {},
      },
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async checkOut() {
      let response = await this.$store.dispatch("purchase", { orders: this.orders });
      if (response) this.close(true);
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.orders = this.ordersProp;
    this.totalPrice = this.totalPriceProp;
  },
};
</script>

<style scoped></style>
