<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon id="icon">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("newVersionInfo") }}</h2>
      <hr class="mb-8" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('version')" rules="required|max:32">
          <v-text-field
            v-model="versionInfoModel.version"
            :label="translator('version')"
            :error-messages="errors"
            required
            outlined
            counter="32"
          ></v-text-field>
        </ValidationProvider>

        <hr class="mb-5" />

        <v-btn @click.prevent="versionInfoCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("create") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "@/helpers/Components/MessageBox";
import VersionInfoService from "@/services/VersionInfoService";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  data() {
    return {
      versionInfoModel: {},
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async versionInfoCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new VersionInfoService().create(this.versionInfoModel);
        if (response.success) this.close("closed", true);
        else {
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
    closeDialog() {
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

<style scoped>
#icon {
  float: right;
}
</style>
