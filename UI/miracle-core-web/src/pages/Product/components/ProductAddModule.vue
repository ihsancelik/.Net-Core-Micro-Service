<template>
  <v-card class="pa-10" style="margin: -50px 0 0;" elevation="5">
    <v-main>
      <h2 class="text-md-center">{{ translator("productAddModule") }}</h2>
      <hr class="mb-5" />

      <v-select
        v-model="productModuleModel.moduleId"
        :items="productModules"
        item-value="id"
        :placeholder="translator('productModule')"
        outlined
      >
        <template slot="item" slot-scope="data">{{ data.item.name }}</template>
        <template slot="selection" slot-scope="data">{{ data.item.name }}</template>
      </v-select>

      <v-btn @click.prevent="productAddModule" color="primary" dark class="v-btn--block v-size--large mb-2">
        {{ translator("add") }}
      </v-btn>
      <v-btn @click="close(false)" color="red" dark class="v-btn--block v-size--large mb-2">
        {{ translator("close") }}
      </v-btn>
    </v-main>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </v-card>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import ProductModuleService from "@/services/ProductModuleService";
import ProductService from "@/services/ProductService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  props: ["productIdProp"],
  data() {
    return {
      productModules: [],
      productModuleModel: {
        productId: 0,
        moduleId: 0,
      },
      signalModel: {
        changes: false,
        returnValues: undefined,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  methods: {
    async initialize() {
      this.productModuleModel.productId = this.productIdProp;

      let response = await new ProductModuleService().getListAll();
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.productModules = response.list;
    },
    async productAddModule() {
      let response = await new ProductService().productAddModule(
        this.productModuleModel.productId,
        this.productModuleModel.moduleId
      );
      if (response.success) this.close(true);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
        };
        this.dialogEnable = true;
        this.dialogKey += 1;
      }
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.initialize();
  },
};
</script>

<style></style>
