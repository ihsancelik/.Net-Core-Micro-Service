<template>
  <v-container>
    <v-row style="margin: 0 5%;">
      <v-col v-for="(count, index) in orderedCountArray" :key="index" cols="12" xl="4" md="6" sm="6" xs="12">
        <v-card class="cardStyle" :to="count.route">
          <v-sheet class="v-sheet--offset mx-auto" :color="count.color" elevation="12">
            <span>{{ count.value }}</span>
          </v-sheet>
          <v-card-text class="pt-0 text-center">
            <span class="title font-weight-light mb-2">{{ translator(count.name) }}</span>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ContactFormService from "@/services/ContactFormService";
import NewsService from "@/services/NewsService";
import ProductService from "@/services/ProductService";
import SliderService from "@/services/SliderService";
import TicketService from "@/services/TicketService";
import _ from "lodash";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  created() {
    this.initialize();
  },
  data() {
    return {
      countArray: [],
    };
  },
  methods: {
    async initialize() {
      let ticketC = await new TicketService().getCount();
      let sliderC = await new SliderService().getCount();
      let productC = await new ProductService().getCount();
      let newsC = await new NewsService().getCount();
      let contactFormC = await new ContactFormService().getCount();

      this.countArray.length = 0;

      if ((ticketC || sliderC || productC || newsC || contactFormC) === ReturnConstraints.REFRESH)
        await this.initialize();
      else
        this.countArray.push({
          name: "products",
          value: productC.data,
          route: "admin/products",
          color: "success",
          order: 1,
        });
      this.countArray.push({
        name: "sliders",
        value: sliderC.data,
        route: "admin/sliders",
        color: "warning",
        order: 2,
      });
      this.countArray.push({
        name: "news",
        value: newsC.data,
        route: "admin/news",
        color: "lime",
        order: 3,
      });
      this.countArray.push({
        name: "contacts",
        value: contactFormC.data,
        route: "admin/contacts",
        color: "cyan",
        order: 4,
      });
      this.countArray.push({
        name: "ticketsManager",
        value: ticketC.data,
        route: "admin/ticket-res",
        color: "cyan",
        order: 5,
      });
    },
  },
  computed: {
    orderedCountArray() {
      return _.sortBy(this.countArray, "order");
    },
  },
};
</script>

<style scoped>
.cardStyle {
  height: 175px;
  width: 100%;
  margin-bottom: 10px;
}

.v-sheet--offset {
  top: -15px;
  position: relative;
  max-width: calc(100% - 32px);
  text-align: center;
}

.v-sheet--offset span {
  font-size: 75px;
  font-weight: bold;
}
</style>
