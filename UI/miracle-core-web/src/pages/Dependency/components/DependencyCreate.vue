<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newDependency") }}</h2>
      <hr class="mb-3" />

      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="12">
            <v-file-input
              enctype="multipart/form-data"
              accept=".dll"
              :placeholder="translator('libFile')"
              @change="uploadFile"
            ></v-file-input>
          </v-col>
        </v-row>

        <v-row id="rows">
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('libName')" rules="required|max:64">
              <v-text-field
                v-model="dependencyModel.libName"
                :label="translator('libName')"
                :error-messages="errors"
                outlined
                counter="64"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-switch v-model="dependencyModel.isActive" :label="translator('isActive')"></v-switch>

        <hr class="mb-2" />
        <v-btn @click.prevent="dependencyCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import DependencyService from "@/services/DependencyService";

setInteractionMode("eager");

extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      dependencyModel: {},

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
    async dependencyCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        for (let [key, value] of Object.entries(this.dependencyModel)) {
          formData.append(`${key}`, `${value}`);
        }

        formData.append("libFile", this.dependencyModel.libFile);

        let response = await new DependencyService().create(formData);
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
    uploadFile(file) {
      this.dependencyModel.libFile = file;
      let reader = new FileReader();
      reader.readAsDataURL(file);
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
    this.dependencyModel.isActive = false;
  },
};
</script>

<style scoped>
#rows {
  margin-bottom: -6%;
}

#rows1 {
  margin-bottom: -3%;
}
</style>
