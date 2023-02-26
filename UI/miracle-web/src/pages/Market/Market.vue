<template>
  <v-container>
    <v-row style="margin-bottom: 5%; align-items: center; justify-content: center;">
      <v-col
        v-for="(product, i) in orderedProducts"
        :key="product.name + product.version + i"
        xl="3"
        lg="4"
        md="6"
        sm="6"
        xs="12"
        style="justify-items: center; min-width: 400px; max-width: 400px;"
      >
        <product-card :productProp="product" :isMarketItemProp="true" />
      </v-col>
    </v-row>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-container>
</template>

<script>
import { Base } from "@/helpers/RouteConstraints";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import ProductCard from "@/components/Product/ProductCard";
import _ from "lodash";

export default {
  created() {
    this.initialize();
  },
  components: {
    ProductCard,
  },
  data() {
    return {
      products: [],

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
  methods: {
    async initialize() {
      this.products = [];
      let productResponse = await new ProductService().getListAll();
      for (const prod of productResponse.list) {
        let versionResponse = await new VersionInfoService().getById(prod.versionId);
        let imageResponse = await new ProductService().getImage(prod.id);
        prod.version = versionResponse.data;
        let product = prod;
        product.img = Base + imageResponse;
        this.products.push(product);
      }
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      if (changes) {
        this.initialize();
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
  computed: {
    orderedProducts() {
      return _.sortBy(this.products, "order");
    },
  },
};
</script>

<style scoped></style>
