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
import CompanyService from "@/services/CompanyService";
import UserService from "@/services/UserService";
import PriorityService from "@/services/PriorityService";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import DependencyService from "@/services/DependencyService";
import ProductModuleService from "@/services/ProductModuleService";
import { ReturnConstraints } from "@/helpers/Constraints";
import _ from "lodash";

export default {
  data() {
    return {
      countArray: [],
    };
  },
  methods: {
    async initialize() {
      let userC = await new UserService().getCount();
      let companyC = await new CompanyService().getCount();
      let productC = await new ProductService().getCount();
      let priorityC = await new PriorityService().getCount();
      let versionC = await new VersionInfoService().getCount();
      let dependencyC = await new DependencyService().getCount();
      let pModuleC = await new ProductModuleService().getCount();

      this.countArray.length = 0;

      if (
        (userC || companyC || priorityC || productC || versionC || dependencyC || pModuleC) ===
        ReturnConstraints.REFRESH
      )
        await this.initialize();
      else {
        this.countArray.push({
          name: "users",
          value: userC.data,
          route: "/users",
          color: "success",
          order: 1,
        });
        this.countArray.push({
          name: "companies",
          value: companyC.data,
          route: "/companies",
          color: "warning",
          order: 2,
        });
        this.countArray.push({
          name: "products",
          value: productC.data,
          route: "/products",
          color: "blue",
          order: 3,
        });
        this.countArray.push({
          name: "productModules",
          value: pModuleC.data,
          route: "/product-modules",
          color: "lime",
          order: 4,
        });
        this.countArray.push({
          name: "priorities",
          value: priorityC.data,
          route: "/priorities",
          color: "purple",
          order: 5,
        });
        this.countArray.push({
          name: "versionInfos",
          value: versionC.data,
          route: "/version-infos",
          color: "cyan",
          order: 6,
        });
        this.countArray.push({
          name: "dependencies",
          value: dependencyC.data,
          route: "/dependencies",
          color: "red",
          order: 7,
        });
      }
    },
  },
  created() {
    this.initialize();
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
  height: 150px;
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
