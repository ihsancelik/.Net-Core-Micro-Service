<template>
  <v-container>
    <v-card elevation="5">
      <v-layout justify-center>
        <v-row style="margin-top: 2%; justify-content: center;">
          <v-col cols="12" md="8" sm="12">
            <v-carousel cycle height="500" hide-delimiter-background show-arrows-on-hover>
              <v-carousel-item v-for="(slide, i) in orderedSliders" :key="i">
                <v-sheet height="100%">
                  <v-row class="fill-height" align="center" justify="center">
                    <v-col cols="12" md="12" style="text-align: center;">
                      <img :src="slide.img" alt="" style="max-width: 100%;" />
                    </v-col>
                  </v-row>
                </v-sheet>
              </v-carousel-item>
            </v-carousel>
          </v-col>
        </v-row>
      </v-layout>

      <v-row
        style="justify-content: center; margin-top: 10%; margin-bottom: 5%; background-color: #eeeeee;"
        class="elevation-5"
      >
        <v-col
          v-for="(product, i) in orderedProducts"
          :key="product.name + product.version + i"
          cols="12"
          xl="3"
          lg="4"
          md="6"
          sm="12"
          style="justify-items: center; min-width: 400px; max-width: 400px;"
        >
          <product-card :productProp="product" />
        </v-col>
      </v-row>

      <br />
      <br />
    </v-card>
  </v-container>
</template>

<script>
import { Base } from "@/helpers/RouteConstraints";
import _ from "lodash";
import ProductCard from "@/components/Product/ProductCard";
import ProductService from "@/services/ProductService";
import SliderService from "@/services/SliderService";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "../../helpers/Constraints";

export default {
  created() {
    this.initialize();
  },
  components: {
    ProductCard,
  },
  data() {
    return {
      sliders: [],
      products: [],
    };
  },
  methods: {
    async initialize() {
      let sliderResponse = await new SliderService().getListAll();
      let productResponse = await new ProductService().getListAll();

      if ((sliderResponse || productResponse) === ReturnConstraints.REFRESH) await this.initialize();

      let sliders = sliderResponse.list;
      for (const slide of sliders) {
        let imageResponse = await new SliderService().getImage(slide.id);
        let slider = slide;
        slider.img = Base + imageResponse;
        this.sliders.push(slider);
      }

      this.products = [];
      let products = productResponse.list;
      for (const prod of products) {
        let versionResponse = await new VersionInfoService().getById(prod.versionId);
        let imageResponse = await new ProductService().getImage(prod.id);
        prod.version = versionResponse.data;
        let product = prod;
        product.img = Base + imageResponse;
        this.products.push(product);
      }
    },
  },
  computed: {
    orderedSliders() {
      return _.sortBy(this.sliders, "order");
    },
    orderedProducts() {
      return _.sortBy(this.products, "order");
    },
  },
};
</script>
