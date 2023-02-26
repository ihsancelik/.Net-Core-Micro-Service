<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="products"
      sort-by="id"
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
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>
      
      <template #item.description="{ item }">
        <v-content :style="{ maxWidth: '600px', maxHeight: '60px', overflowY: 'scroll', padding: '15px 5px' }">
          {{ item.description }}
        </v-content>
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showDetails(item)">mdi-format-list-bulleted</v-icon>
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md class="mr-2" @click="showDeleteConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-layout>
</template>

<script>
import MessageBox from "../../../helpers/components/MessageBox";
import ProductCreate from "./components/ProductCreate";
import ProductDetails from "./components/ProductDetails";
import ProductEdit from "./components/ProductEdit";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("tag"), value: "tag" },
        { text: this.translator("version"), value: "version" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("price"), value: "price" },
        { text: this.translator("currency"), value: "currency" },
        { text: this.translator("order"), value: "order" },
        { text: this.translator("publishDate"), value: "publishDate" },
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
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,

      loading: true,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    async initialize() {
      this.loading = true;
      this.products = [];
      let productResponse = await new ProductService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (productResponse === ReturnConstraints.REFRESH) await this.initialize();

      for (const product of productResponse.pagedList.list) {
        let versionResponse = await new VersionInfoService().getById(product.versionId);
        product.version = versionResponse.data;
        this.products.push(product);
      }
      this.pagedList = productResponse.pagedList;
    },
    showDetails(product) {
      this.selectedComponent = ProductDetails;
      this.componentProps = { productIdProp: product.id };

      this.openDialog(600);
    },
    showCreate() {
      this.selectedComponent = ProductCreate;
      this.componentProps = null;

      this.openDialog(550);
    },
    showEdit(product) {
      this.selectedComponent = ProductEdit;
      this.componentProps = { productIdProp: product.id };

      this.openDialog(550);
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
    async deleteProduct(product) {
      const index = this.products.indexOf(product);
      let response = await new ProductService().delete(product.id);
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
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteProduct(this.selectedProduct);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
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

<style scoped></style>
