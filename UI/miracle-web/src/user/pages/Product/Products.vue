<template>
  <v-container>
    <v-row style="margin-left: 40%;"> <h1>Your Products</h1> </v-row>

    <v-row style="margin: 5%;">
      <v-col cols="9" xl="4" md="6" sm="6" xs="12" v-for="(detail, i) in 3" :key="i">
        <v-card class="cardStyle" to="user/products">
          <v-sheet class="v-sheet--offset mx-auto" color="cyan" elevation="12">
            <div>
              <span> Product {{ detail }}</span>
            </div>
          </v-sheet>
          <v-card-text class="pt-0">
            <div class="title font-weight-light mb-2 text-center">{{ translator("products") }}</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ProductService from "@/services/ProductService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  created() {
    this.initialize();
  },
  data() {
    return {
      products: [],
    };
  },
  methods: {
    async initialize() {
      let response = await new ProductService().productGetByTag({ tag: this.$route.params.tag });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.products = response.list;
    },
  },
};
</script>
