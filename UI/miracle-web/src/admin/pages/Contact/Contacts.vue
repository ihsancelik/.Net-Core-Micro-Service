<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="contactMessages"
      sort-by="id"
      class="elevation-5"
      :server-items-length.sync="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("contacts") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
        </v-toolbar>
      </template>

      <template #item.message="{ item }">
        <div :style="{ maxWidth: '400px', maxHeight: '60px', overflowY: 'scroll', padding: '15px 5px' }">
          {{ item.message }}
        </div>
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
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
import ContactFormService from "../../../services/ContactFormService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "fullName" },
        { text: this.translator("email"), value: "email" },
        { text: this.translator("phone"), value: "phone" },
        { text: this.translator("message"), value: "message" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      contactMessages: [],
      selectedContactMessage: {},
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
      let response = await new ContactFormService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.contactMessages = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showDeleteConfirm(message) {
      this.selectedContactMessage = message;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteMessage(message) {
      const index = this.contactMessages.indexOf(message);
      let response = await new ContactFormService().delete(message.id);

      if (response.success) this.contactMessages.splice(index, 1);
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
        if (changes) this.deleteMessage(this.selectedContactMessage);
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
