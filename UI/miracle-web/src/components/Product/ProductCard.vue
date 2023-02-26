<template>
  <v-card class="mx-auto" max-width="95%">
    <v-img :src="product.img" style="margin:5%; height:200px"/>
    <hr />
    <v-card-title>
      {{ product.name }} <span class="mx-2" style="font-size: small;">( v{{ product.version }} )</span> <v-spacer />
      <v-chip v-if="isMarketItemProp">
        {{ product.price + " " + product.currency }}
      </v-chip>
    </v-card-title>
    <!-- div kayıyor details açıldığında -->
    <v-card-text>
      <v-btn icon @click="show = !show" style="float:right">
        <v-icon>{{show ? 'mdi-chevron-up' : 'mdi-chevron-down'}}</v-icon>
      </v-btn>
      <v-expand-transition>
        <div v-show="show">
          {{ product.description }}
        </div>
      </v-expand-transition>
      
    </v-card-text>
    <v-card-actions>
      <v-btn small v-if="isMarketItemProp" text outlined @click="addToCart">
        {{ translator("addToCart") }}
      </v-btn>

      <v-btn
        small
        text
        :to="
          $router.currentRoute.path !== '/qt/market'
            ? 'product-details/tag=' + product.tag
            : '/qt/product-details/tag=' + product.tag
        "
      >
        {{ translator("details") }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  name: "ProductCard",
  props: ["productProp", "isMarketItemProp"],
  data() {
    return {
      product: {},
      show: false,
    };
  },
  created() {
    this.product = this.productProp;
  },
  methods: {
    addToCart() {
      this.$store.dispatch("addToCart", this.product);
    },
  },
};
</script>

<style scoped></style>
