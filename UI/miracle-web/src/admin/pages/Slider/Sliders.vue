<template>
  <v-layout child-flex class="ml-5 mr-5">
    <v-data-table
      :headers="headers"
      :items="sliders"
      sort-by="id"
      class="elevation-5"
      :server-items-length="pagedList.rowCount"
      :items-per-page.sync="paginationModel.pageSize"
      :page.sync="paginationModel.page"
      :loading="loading"
    >
      <template #top>
        <v-toolbar flat color="white">
          <v-toolbar-title>{{ translator("sliders") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>

          <v-btn @click="showCreate" color="primary" dark class="mb-2">{{ translator("create") }}</v-btn>
        </v-toolbar>
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
import MessageBox from "@/helpers/components/MessageBox";
import SliderCreate from "./components/SliderCreate";
import SliderEdit from "./components/SliderEdit";
import SliderService from "@/services/SliderService";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  data() {
    return {
      headers: [
        { text: this.translator("name"), value: "name" },
        { text: this.translator("order"), value: "order" },
        { text: this.translator("isActive"), value: "isActive" },
        { text: this.translator("actions"), value: "actions", sortable: false },
      ],
      sliders: [],
      selectedSlider: {},
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
      let response = await new SliderService().getList(this.paginationModel).finally(() => {
        this.loading = false;
      });
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.sliders = response.pagedList.list;
      this.pagedList = response.pagedList;
    },
    showCreate() {
      this.selectedComponent = SliderCreate;
      this.componentProps = null;

      this.openDialog(550);
    },
    showEdit(slider) {
      this.selectedComponent = SliderEdit;
      this.componentProps = { sliderIdProp: slider.id };

      this.openDialog(550);
    },
    showDeleteConfirm(slider) {
      this.selectedSlider = slider;
      this.selectedComponent = MessageBox;
      this.componentProps = {
        messageTitleProp: this.translator("deleteTitle"),
        messageTextProp: this.translator("deleteMessage"),
        cancelButtonVisibleProp: true,
      };

      this.openDialog();
    },
    async deleteSlider(slider) {
      const index = this.sliders.indexOf(slider);
      let response = await new SliderService().delete(slider.id);
      if (response.success) this.sliders.splice(index, 1);
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
        if (changes) this.deleteSlider(this.selectedSlider);
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
