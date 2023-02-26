<template>
  <v-card class="pa-10" style="margin: -50px 0 0;" elevation="5">
    <v-main>
      <h2 class="text-md-center">{{ translator("productAddVersion") }}</h2>
      <hr class="mb-5" />

      <v-select
        v-model="versionModel.priorityId"
        :items="priorities"
        item-text="name"
        item-value="id"
        :placeholder="translator('priorities')"
        outlined
      ></v-select>
      <v-select
        v-model="versionModel.versionInfoId"
        :items="orderedVersionInfos"
        item-value="id"
        :placeholder="translator('versionInfo')"
        outlined
      >
        <template slot="item" slot-scope="data">{{ data.item.version }}</template>
        <template slot="selection" slot-scope="data">{{ data.item.version }}</template>
      </v-select>

      <v-btn @click.prevent="productAddVersion" color="primary" dark class="v-btn--block v-size--large mb-2">
        {{ translator("add") }}
      </v-btn>
      <v-btn @click="close(false)" color="red" dark class="v-btn--block v-size--large mb-2">
        {{ translator("close") }}
      </v-btn>
    </v-main>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </v-card>
</template>

<script>
import MessageBox from "@/helpers/Components/MessageBox";
import PriorityService from "@/services/PriorityService";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";
import { ReturnConstraints } from "@/helpers/Constraints";
import _ from "lodash";

export default {
  props: ["productIdProp"],
  data() {
    return {
      priorities: [],
      versionInfos: [],
      versionModel: {
        productId: 1,
        versionInfoId: null,
        priorityId: null,
      },
      signalModel: {
        changes: false,
        returnValues: undefined,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  methods: {
    async initialize() {
      let priorityResponse = await new PriorityService().getListAll();
      let versionResponse = await new VersionInfoService().getListAll();

      if ((priorityResponse || versionResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.priorities = priorityResponse.list;
      this.versionInfos = versionResponse.list;
    },
    async productAddVersion() {
      this.versionModel.productId = this.productIdProp;
      let response = await new ProductService().productAddVersion(
        this.versionModel.productId,
        this.versionModel.versionInfoId,
        this.versionModel.priorityId
      );

      if (response.success) this.close(true);
      else {
        this.selectedComponent = MessageBox;
        this.componentProps = {
          messageTitleProp: this.translator("failed"),
          messageTextProp: response.message,
        };
        this.dialogEnable = true;
        this.dialogKey += 1;
      }
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.initialize();
  },
  computed: {
    orderedVersionInfos() {
      return _.sortBy(this.versionInfos, ["version"]);
    },
  },
};
</script>

<style></style>
