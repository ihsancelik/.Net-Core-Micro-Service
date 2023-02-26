<template>
  <v-container fluid>
    <v-row v-for="(product, index) in products" :key="index" style="margin-bottom: 1%;">
      <v-card class="mx-auto" min-width="900" max-width="900" raised="true" elevation="10">
        <v-card-title>
          <v-img width="100%" :src="product.img"> </v-img>
        </v-card-title>
        <br />
        <v-card-subtitle class="pb-0 text-center" style="font-size: 34px;">{{ product.name }}</v-card-subtitle>
        <br />
        <v-card-text class="text--primary">
          <v-container fluid>
            <v-row>
              <v-col cols="12" md="12">
                <v-col cols="4" v-for="(detail, i) in product.productDetails" :key="i">
                  <v-card class="ma-3 pa-6" height="250">
                    <v-card-title style="font-size: 16px;">{{ detail.title }}</v-card-title>
                    <v-card-text style="font-size: 14px;">{{ detail.content }} </v-card-text>
                  </v-card>
                </v-col>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>

        <hr style="margin-left: 30px; margin-right: 30px;" />
      </v-card>
    </v-row>
  </v-container>
</template>

<script>
import ProductService from "@/services/ProductService";
import { Base } from "@/helpers/RouteConstraints";

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
      let productResponse = await new ProductService().productGetByTag({ tag: this.$route.params.tag });
      for (const item of productResponse.list) {
        let imageResponse = await new ProductService().getImage(item.id);
        item.img = Base + imageResponse;
        this.products.push(item);
      }
    },
  },
};
</script>

<style scoped></style>
