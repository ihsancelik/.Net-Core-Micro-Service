<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newCompany") }}</h2>
      <hr class="mb-5" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:128">
          <v-text-field
            v-model="companyModel.name"
            :label="translator('name')"
            :error-messages="errors"
            required
            outlined
          ></v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('address')" rules="required|max:512">
          <v-textarea
            v-model="companyModel.address"
            :label="translator('address')"
            :error-messages="errors"
            required
            outlined
          ></v-textarea>
        </ValidationProvider>

        <v-textarea v-model="companyModel.location" :label="translator('location')" required outlined></v-textarea>

        <ValidationProvider #default="{ errors }" :name="translator('phone')" rules="required|min:10|max:11|numeric">
          <v-text-field
            v-model="companyModel.phoneNumber"
            :label="translator('phone')"
            :error-messages="errors"
            required
            outlined
            counter="11"
          ></v-text-field>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click.prevent="companyCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import CompanyService from "@/services/CompanyService";

setInteractionMode("eager");
extend("numeric", { ...rules.numeric, message: "{_field_} can be numeric" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("min", { ...rules.min, message: "{_field_} may not be less than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      companyModel: {},
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
    async companyCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new CompanyService().create(this.companyModel);
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
