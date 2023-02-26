<template>
  <v-card>
    <v-data-table
      :headers="headers"
      :items="productModules"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="dark">
          <v-toolbar-title>{{ translator("productModules") }}</v-toolbar-title>

          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-btn class="mb-2 ml-1 mr-1" color="primary" dark @click="showAddProductModule">
            {{ translator("add") }}
          </v-btn>
          <v-btn class="mb-2" color="red" dark @click="close(false)">{{ translator("close") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.actions="{ item }">
        <v-icon md @click="showRemoveConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-card>
</template>

<script>
import ProductAddModule from "./ProductAddModule";
import MessageBox from "@/helpers/Components/MessageBox";
import ProductModuleService from "@/services/ProductModuleService";
import ProductService from "@/services/ProductService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  props: ["productIdProp"],
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      productModules: [],
      selectedProductModule: {},
      selectedProductModuleId: 0,
      productId: 0,
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      pagedList: {},
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: {},

      loading: true,
    };
  },
  methods: {
    async initialize() {
      this.productModules = [];
      this.productId = this.productIdProp;
      this.loading = true;

      this.productModules = [];
      let response = await new ProductModuleService()
        .getListByProductId(this.productId, this.paginationModel)
        .finally(() => {
          this.loading = false;
        });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.productModules = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showAddProductModule() {
      this.selectedComponent = ProductAddModule;
      this.componentProps = {
        productIdProp: this.productId,
      };

      this.openDialog();
    },
    showRemoveConfirm(productModule) {
      this.selectedProductModule = productModule;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.dialogEnable = true;
    },
    async removeModule(productModule) {
      let response = await new ProductService().productRemoveModule(this.productId, productModule.id);
      if (response.success) {
        this.productModules.forEach((item) => {
          if (item.id === productModule.id) this.productModules.splice(this.productModules.indexOf(productModule), 1);
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
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.removeModule(this.selectedProductModule);
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

<style scoped></style>
