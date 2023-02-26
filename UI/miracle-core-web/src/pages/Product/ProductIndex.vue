<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="products"
      sort-by="State"
      group-by="tag"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("products") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer />

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>
      <template #item.description="{ item }">
        <div class="description">{{ item.description }}</div>
      </template>
      <template #item.publishDate="{ item }">
        {{ new Date(item.publishDate).toISOString().substr(0, 19).replace("T", " ") }}
      </template>

      <template #item.isPlugin="{ item }">
        {{ item.isPlugin ? translator("plugin") : translator("product") }}
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showVersionInfos(item)">mdi-apps-box</v-icon>
        <v-icon md class="mr-2" @click="showModules(item)">mdi-view-module</v-icon>
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-layout>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import ProductCreate from "./components/ProductCreate";
import ProductEdit from "./components/ProductEdit";
import ProductVersionInfo from "./components/ProductVersionInfo";
import ProductModules from "./components/ProductModules";
import ProductService from "@/services/ProductService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("tag"), value: "tag" },
        { text: this.translator("publishDate"), value: "publishDate" },
        { text: this.translator("type"), value: "isPlugin" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      products: [],
      selectedProduct: {},
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: null,

      loading: true,
    };
  },
  methods: {
    async initialize() {
      this.loading = true;
      let response = await new ProductService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.pagedList = response.pagedList;
      this.products = response.pagedList.list;
    },
    showCreate() {
      this.selectedComponent = ProductCreate;
      this.componentProps = null;

      this.openDialog();
    },
    showEdit(product) {
      this.selectedComponent = ProductEdit;
      this.componentProps = {
        productIdProp: product.id,
      };

      this.openDialog();
    },
    showVersionInfos(product) {
      this.selectedComponent = ProductVersionInfo;
      this.componentProps = {
        productIdProp: product.id,
      };

      this.openDialog("50%");
    },
    showModules(product) {
      this.selectedComponent = ProductModules;
      this.componentProps = {
        productIdProp: product.id,
      };

      this.openDialog("50%");
    },
    showDeleteConfirm(product) {
      this.selectedProduct = product;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteProduct(products) {
      const index = this.products.indexOf(products);
      let response = await new ProductService().delete(products.id);
      if (response.success) this.products.splice(index, 1);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      this.dialogEnable = false;
      this.dialogKey += 1;

      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteProduct(this.selectedProduct);
      }
    },
    setReturnValues(values) {
      return values;
    },
  },
  created() {
    this.initialize();
  },
  watch: {
    "paginationModel.page"() {
      this.initialize();
    },
    "paginationModel.pageSize"() {
      this.initialize();
    },
  },
};
</script>

<style scoped>
.description {
  max-width: 300px;
  max-height: 60px;
  overflow-y: scroll;
  padding: 15px 15px;
}
</style>
