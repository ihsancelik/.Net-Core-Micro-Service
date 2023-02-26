<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="productTags"
      sort-by="ID"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("productTags") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="createProductTag" color="primary" dark class="mb-2">
            {{ translator("create") }}
          </v-btn>
        </v-toolbar>
      </template>

      <template #item.actions="{ item }">
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
import ProductTagCreate from "./components/ProductTagCreate";
import ProductTagEdit from "./components/ProductTagEdit";
import ProductTagService from "@/services/ProductTagService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: "Id", value: "id" },
        { text: this.translator("tag"), value: "tag" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      productTags: [],
      selectedProductTag: {},
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
      let response = await new ProductTagService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.productTags = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    createProductTag() {
      this.selectedComponent = ProductTagCreate;
      this.componentProps = null;

      this.openDialog();
    },
    showEdit(productTag) {
      this.selectedComponent = ProductTagEdit;
      this.componentProps = {
        productTagIdProp: productTag.id,
      };

      this.openDialog();
    },
    showDeleteConfirm(productTag) {
      this.selectedProductTag = productTag;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteProductTag(productTag) {
      const index = this.productTags.indexOf(productTag);
      let response = await new ProductTagService().delete(productTag.id);
      if (response.success) this.productTags.splice(index, 1);
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
        if (changes) this.deleteProductTag(this.selectedProductTag);
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
