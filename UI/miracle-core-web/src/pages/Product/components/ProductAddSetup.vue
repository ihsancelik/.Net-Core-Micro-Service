<template>
  <v-card class="pl-10 pr-10" elevation="5">
    <v-main class="text-center" style="margin-top: -50px;">
      <h2>{{ translator("uploadSetups") }}</h2>
      <h4>{{ translator("version") }}: {{ this.versionInfo.version }}</h4>
      <hr class="ma-3" />

      <v-list v-for="platform in orderedPlatform" :key="platform.id">
        <h3 class="text-center">
          {{ platform.name }} -
          <v-icon>{{ platform.setupExist ? "mdi-check-bold" : "mdi-close-circle" }}</v-icon>
        </h3>
        <v-file-input
          outlined
          dense
          accept=".zip"
          :placeholder="translator('setupFile')"
          @change="addFile(platform.id, $event)"
        >
          <template #append-outer>
            <v-progress-circular
              :key="platform.id"
              v-if="fileModels.find((s) => s.platformId === platform.id && s.file !== null)"
              rotate="-90"
              size="37"
              color="green"
              style="margin-top: -6px;"
              :value="getProgress(platform.id)"
              >{{ getProgress(platform.id) }}
            </v-progress-circular>
          </template>
        </v-file-input>
      </v-list>

      <v-btn @click="uploadSetups" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import PlatformService from "@/services/PlatformService";
import ProductService from "@/services/ProductService";
import VersionInfoService from "@/services/VersionInfoService";

import _ from "lodash";
import { ReturnConstraints } from "@/helpers/Constraints";

export default {
  props: ["productIdProp", "versionInfoIdProp"],
  data() {
    return {
      platforms: [],
      versionInfo: {},
      fileModels: [
        {
          platformId: 0,
          productId: 0,
          versionInfoId: 0,
          file: undefined,
        },
      ],
      signalModel: {
        changes: false,
        returnValues: undefined,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,

      progress: {
        uploadProgressL: 0,
        uploadProgressM: 0,
        uploadProgressW: 0,
      },
    };
  },
  methods: {
    async initialize() {
      let platformResponse = await new PlatformService().getListByProductId(this.productIdProp);
      let versionResponse = await new VersionInfoService().getById(this.versionInfoIdProp);

      if ((platformResponse || versionResponse) === ReturnConstraints.REFRESH) await this.initialize();

      this.versionInfo = versionResponse.data;
      for (const platform of platformResponse.list) {
        let productResponse = await new ProductService().productExistSetup(
          platform.id,
          this.productIdProp,
          this.versionInfoIdProp
        );
        platform.setupExist = productResponse.data;
        this.platforms.push(platform);
      }
    },
    addFile(platformId, file) {
      const fileModel = this.fileModels.find((s) => s.platformId === platformId);
      let index = this.fileModels.indexOf(fileModel);

      if (index !== -1) {
        this.fileModels.splice(index, 1);
      }

      if (file !== undefined)
        this.fileModels.push({
          platformId: platformId,
          productId: this.productIdProp,
          versionInfoId: this.versionInfoIdProp,
          file: file,
        });
    },
    async uploadSetups() {
      const fileModels = this.fileModels;
      let fileCount = fileModels.length;
      for (let i = 0; i < fileCount; i++) {
        let formData = new FormData();
        let fileModel = fileModels[i];
        formData.append("file", fileModel.file);

        let response = await new ProductService().productAddSetup(
          this.fileModels[i].platformId,
          this.fileModels[i].productId,
          this.fileModels[i].versionInfoId,
          formData
        );

        if (response.success) {
          setTimeout(() => {
            this.close(true);
          }, 2000);
        } else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
    },
    getProgress(platformId) {
      if (platformId === 1) return this.progress.uploadProgressL;
      if (platformId === 2) return this.progress.uploadProgressM;
      if (platformId === 3) return this.progress.uploadProgressW;
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
  watch: {
    "$store.state.progress.uploadProgressL"(object) {
      this.progress.uploadProgressL = object;
    },
    "$store.state.progress.uploadProgressM"(object) {
      this.progress.uploadProgressM = object;
    },
    "$store.state.progress.uploadProgressW"(object) {
      this.progress.uploadProgressW = object;
    },
  },
  computed: {
    orderedPlatform() {
      return _.sortBy(this.platforms, "id"); //Platformları sıraya diziyor. Sürekli değişmesi engellendi.
    },
  },
  created() {
    this.fileModels = [];
    this.versionInfoId = this.selectedVersionInfoId;
    this.initialize();
  },
};
</script>

<style></style>
