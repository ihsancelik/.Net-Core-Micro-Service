<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="smtpSettings"
      sort-by="ID"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("smtpSettings") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.enableSSL="{ item }">
        <v-icon>{{ item.enableSSL ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
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
import SMTPSettingEdit from "./components/SMTPSettingEdit";
import SMTPSettingCreate from "./components/SMTPSettingCreate";
import SmtpSettingService from "@/services/SmtpSettingService";
import MessageBox from "@/helpers/Components/MessageBox";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: "Id", value: "id" },
        { text: "Host", value: "host" },
        { text: "Port", value: "port" },
        { text: this.translator("email"), value: "email" },
        { text: "Enable SSL", value: "enableSSL" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      smtpSettings: [],
      selectedSmtpSetting: {},
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
      let response = await new SmtpSettingService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.smtpSettings = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = SMTPSettingCreate;
      this.componentProps = null;

      this.openDialog();
    },
    showEdit(smtpSetting) {
      this.selectedComponent = SMTPSettingEdit;
      this.componentProps = {
        smtpSettingIdProp: smtpSetting.id,
      };

      this.openDialog();
    },
    showDeleteConfirm(smtpSetting) {
      this.selectedSmtpSetting = smtpSetting;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog(400);
    },
    async deleteSmtpSetting(smtpSetting) {
      const index = this.smtpSettings.indexOf(smtpSetting);
      let response = await new SmtpSettingService().delete(smtpSetting.id);
      if (response.success) this.smtpSettings.splice(index, 1);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    openDialog(width = 500) {
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
        if (changes) this.deleteSmtpSetting(this.selectedSmtpSetting);
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
