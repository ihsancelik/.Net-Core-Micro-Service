<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newProductModule") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:164">
          <v-text-field
            v-model="productModule.name"
            :label="translator('name')"
            :error-messages="errors"
            required
            outlined
            counter="164"
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider
          #default="{ errors }"
          :name="translator('description')"
          rules="required|max:512"
        >
          <v-textarea
            v-model="productModule.description"
            :label="translator('description')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          ></v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click.prevent="productModuleCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import ProductModuleService from "@/services/ProductModuleService";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      productModule: {},
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
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async productModuleCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new ProductModuleService().create(this.productModule);
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

<style scoped></style>
