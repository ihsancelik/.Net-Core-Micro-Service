<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="priorities"
      sort-by="State"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("priorities") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
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
import PriorityCreate from "./components/PriorityCreate";
import PriorityEdit from "./components/PriorityEdit";
import PriorityService from "@/services/PriorityService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("state"), value: "state" },
        { text: this.translator("description"), value: "description" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      priorities: [],
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: 5,
      },
      selectedPriority: {},
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
      let response = await new PriorityService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.priorities = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = PriorityCreate;
      this.componentProps = null;

      this.openDialog();
    },
    showEdit(priority) {
      this.selectedComponent = PriorityEdit;
      this.componentProps = {
        priorityIdProp: priority.id,
      };

      this.openDialog();
    },
    showDeleteConfirm(news) {
      this.selectedPriority = news;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deletePriority(priority) {
      const index = this.priorities.indexOf(priority);

      let response = await new PriorityService().delete(priority.id);
      if (response.success) this.priorities.splice(index, 1);
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
        if (changes) this.deletePriority(this.selectedPriority);
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
