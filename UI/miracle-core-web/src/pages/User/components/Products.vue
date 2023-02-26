<template>
  <v-card>
    <v-layout child-flex>
      <v-data-table
        :headers="headers"
        :items="products"
        item-key="id"
        sort-by="Id"
        group-by="tag"
        class="elevation-5 mx-auto"
        :server-items-length="pagedList.rowCount"
        :items-per-page.sync="paginationModel.pageSize"
        :page.sync="paginationModel.page"
        :loading="loading"
      >
        <template #top>
          <v-toolbar flat color="white">
            <v-toolbar-title style="min-width: 60px;">
              {{ username + "'s " + translator("products") }}
            </v-toolbar-title>

            <v-divider class="mx-4" inset vertical></v-divider>
            <v-spacer></v-spacer>

            <v-btn color="primary" dark class="mb-2" @click="showAddProduct">
              {{ translator("add") }}
            </v-btn>

            <div class="ml-1 mr-1"></div>

            <v-btn color="red" dark class="mb-2" @click="close(false)">{{ translator("close") }}</v-btn>
          </v-toolbar>
        </template>

        <template #item.description="{ item }">
          <div class="description">
            {{ item.description }}
          </div>
        </template>

        <template #item.demoCount="{ item }">
          <v-chip
            v-if="item.isDemo"
            class="chip"
            :style="item.demoCount === 0 ? 'color:white;background-color:red' : 'color:white;background-color:limegreen'">
            {{ item.demoCount }}
          </v-chip>
          <v-icon v-else>mdi-check-bold</v-icon>
        </template>

        <template #item.isPlugin="{ item }">
          {{ item.isPlugin ? translator("plugin") : translator("product") }}
        </template>

        <template #item.isActive="{ item }">
          <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
        </template>

        <template #item.actions="{ item }">
          <v-icon md class="mr-2" @click="showModules(item)">mdi-view-module</v-icon>
          <v-icon md class="mr-2" @click="editProduct(item)">mdi-pencil</v-icon>
          <v-icon md class="mr-2" @click="showDeleteConfirm(item)">mdi-delete</v-icon>
        </template>
      </v-data-table>

      <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
        <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
      </v-dialog>
    </v-layout>
  </v-card>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import ProductAdd from "./ProductAdd";
import Modules from "./Modules";
import UserService from "@/services/UserService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  props: ["userIdProp", "usernameProp"],
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("tag"), value: "tag" },
        { text: this.translator("demoInfo"), value: "demoCount" },
        { text: this.translator("type"), value: "isPlugin" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      userId: null,
      username: "",
      products: [],
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
        searchFilter: "",
        propertyName: "",
        filterType: "",
      },
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 1,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: {},

      loading: true,
    };
  },
  created() {
    this.userId = this.userIdProp;
    this.username = this.usernameProp;
    this.initialize();
  },
  methods: {
    async initialize() {
      this.loading = true;
      this.products = [];

      let productResponse = await new UserService().getUserProducts(this.userId, this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (productResponse === ReturnConstraints.REFRESH) await this.initialize();

      const products = productResponse.pagedList.list;

      for (const product of products) {
        let limitResponse = await new UserService().getProductLimitation(this.userId, product.id);
        if (limitResponse.data !== null) {
          product.isActive = limitResponse.data.isActive;
          product.isDemo = limitResponse.data.isDemo;
          const diffTime = new Date(limitResponse.data.demoEndDate) - new Date();
          const diffDay = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
          product.demoCount = diffDay < 0 ? 0 : diffDay;
          this.products.push(product);
        }
      }
    },
    showAddProduct() {
      this.selectedComponent = ProductAdd;
      this.componentProps = { userIdProp: this.userId };

      this.openDialog();
    },
    showModules(product) {
      this.selectedComponent = Modules;
      this.componentProps = {
        userIdProp: this.userId,
        usernameProp: this.username,
        productIdProp: product.id,
      };

      this.openDialog("50%");
    },
    editProduct(product) {
      this.selectedComponent = ProductAdd;
      this.componentProps = { userIdProp: this.userId, productIdProp: product.id };

      this.openDialog();
    },
    showDeleteConfirm(userProduct) {
      this.selectedProduct = userProduct;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteProduct(product) {
      let response = await new UserService().removeUserProduct(this.userId, product.id);
      if (response.success) {
        this.products.forEach((item) => {
          if (item.id === product.id) this.products.splice(this.products.indexOf(product), 1);
        });
      } else {
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

      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteProduct(this.selectedProduct);
      }
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
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

<style scoped>
.description {
  max-width: 400px;
  max-height: 60px;
  overflow-y: scroll;
  padding: 15px 15px;
}

.chip {
  align-items: center;
  text-align: center;
}
</style>
