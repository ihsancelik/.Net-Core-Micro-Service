<template>
  <v-content>
    <v-data-table :headers="headers" :items="productDetails" sort-by="id" class="elevation-5">
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("product_details") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("add") }}</v-btn>
          <v-btn @click="close(true)" outlined class="ml-4 mb-2">{{ "X" }}</v-btn>
        </v-toolbar>
      </template>
      <template #item.content="{ item }">
        <div :style="{ maxWidth: '200px', maxHeight: '60px', overflowY: 'scroll', padding: '15px 5px' }">
          {{ item.content }}
        </div>
      </template>

      <template #item.actions="{ item }">
        <v-icon md class="mr-2" @click="showEdit(item)">mdi-pencil</v-icon>
        <v-icon md class="mr-2" @click="showRemoveConfirm(item)">mdi-delete</v-icon>
      </template>
    </v-data-table>
    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-content>
</template>

<script>
import MessageBox from "../../../../helpers/components/MessageBox";
import ProductService from "../../../../services/ProductService";

export default {
  props: ["productIdProp"],
  data() {
    return {
      productId: 0,
      headers: [
        { text: this.translator("title"), value: "title" },
        { text: this.translator("content"), value: "content" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      productDetails: [],
      selectedProductDetail: {},
      signalModel: {
        changes: false,
        returnValues: undefined,
      },
      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: null,
      componentProps: null,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    async initialize() {
      this.productDetails = [];
      this.productId = this.productIdProp;
      let response = await new ProductService().getById(this.productId);
      this.productDetails = response.data.productDetails;
    },
    showCreate() {
      this.selectedComponent = () => import("./ProductDetailAdd");
      this.componentProps = { productIdProp: this.productId };

      this.openDialog(400);
    },
    showEdit(productDetail) {
      this.selectedComponent = () => import("./ProductDetailAdd");
      this.componentProps = { productIdProp: this.productId, productDetailProp: productDetail };

      this.openDialog(400);
    },
    showRemoveConfirm(productDetail) {
      this.selectedProductDetail = productDetail;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async removeProductDetail(productDetail) {
      const index = this.productDetails.indexOf(productDetail);
      let response = await new ProductService().removeProductDetail({
        productId: this.productId,
        productDetailId: this.selectedProductDetail.id,
      });
      if (response.success) this.productDetails.splice(index, 1);
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
      if (changes) {
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.removeProductDetail(this.selectedProductDetail);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
};
</script>

<style scoped></style>
