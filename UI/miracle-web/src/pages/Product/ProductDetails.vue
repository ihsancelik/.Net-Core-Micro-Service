<template>
  <v-container>
    <v-row v-for="(product, index) in products" :key="index" style="margin-bottom: 1%;">
      <v-card
        class="mx-auto"
        min-width="90%"
        max-width="90%"
        min-height= "300px"
        :raised="true"
        elevation="10"
        style="text-align: -webkit-center;"
      >
        <v-row style="margin-top:2%"> 
          <v-col md="1"></v-col>
          <v-col md="3">
            <v-img :src="product.img" class="img" />
          </v-col>
          <v-col md="7" style="float: left;">
            <v-card-subtitle class="pb-0 float:left mt-5" style="font-size: 34px;">{{ product.name }}</v-card-subtitle>
            <v-card-title style="font-size: 16px;">{{ product.description }}</v-card-title>
             <!--  <hr style="margin-left: 30px; margin-right: 30px;" />
            <h3 style="font-size: 30px; float:right; margin-left: 5%;"> {{ translator("price") }}: {{ product.price }} {{ product.currency }}</h3> -->
          </v-col>
        </v-row>
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
      let tag = this.$route.params.tag;
      let productResponse = await new ProductService().productGetByTag(tag);
      for (const item of productResponse.list) {
        let imageResponse = await new ProductService().getImage(item.id);
        item.img = Base + imageResponse;
        this.products.push(item);
      }
    },
  },
};
</script>

<style scoped>
.img {
  float: center;
  min-width: 90%;
  max-width: 90%;
  height: 90%;
  margin-top:2%;
}
</style>
