<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="notices"
      sort-by="id"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("notices") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
      </template>

      <template #item.text="{ item }">
        <div :style="{ maxWidth: '600px', maxHeight: '60px', overflowY: 'scroll', padding: '15px 5px' }">
          {{ item.text }}
        </div>
      </template>

      <template #item.startDate="{ item }">
        {{ new Date(item.startDate).toISOString().substr(0, 19).replace("T", " ") }}
      </template>

      <template #item.endDate="{ item }">
        {{ new Date(item.endDate).toISOString().substr(0, 19).replace("T", " ") }}
      </template>

      <template #item.isActive="{ item }">
        <v-icon>{{ item.isActive ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
      </template>

      <template #item.actions="{ item }">
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
import MessageBox from "@/helpers/Components/MessageBox";
import NoticeCreate from "./components/NoticeCreate";
import NoticeEdit from "./components/NoticeEdit";
import NoticeService from "@/services/NoticeService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: "Id", value: "id" },
        { text: this.translator("text"), value: "text" },
        { text: this.translator("startDate"), value: "startDate" },
        { text: this.translator("endDate"), value: "endDate" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      notices: [],
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      selectedNotice: {},
      pagedList: {},
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
      let response = await new NoticeService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.notices = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = NoticeCreate;
      this.componentProps = null;

      this.dialogEnable = true;
    },
    showEdit(notice) {
      this.selectedComponent = NoticeEdit;
      this.componentProps = {
        noticeIdProp: notice.id,
      };

      this.dialogEnable = true;
    },
    showDeleteConfirm(company) {
      this.selectedNotice = company;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.dialogEnable = true;
    },
    async deleteNotice(notice) {
      const index = this.notices.indexOf(notice);
      let response = await new NoticeService().delete(notice.id);
      if (response.success) this.notices.splice(index, 1);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
          cancelButtonVisibleProp: false,
        };
      }
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }
      if (this.selectedComponent === MessageBox) {
        if (changes) this.deleteNotice(this.selectedNotice);
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
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

<style scoped></style>
