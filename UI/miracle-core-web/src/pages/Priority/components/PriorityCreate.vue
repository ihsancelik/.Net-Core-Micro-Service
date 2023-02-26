<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newPriority") }}</h2>
      <hr class="mb-4" />
      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:128">
          <v-text-field
            v-model="priorityModel.name"
            :error-messages="errors"
            :label="translator('name')"
            required
            outlined
            counter="128"
          >
          </v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('state')" rules="required|max:128">
          <v-text-field
            v-model="priorityModel.state"
            :error-messages="errors"
            :label="translator('state')"
            required
            outlined
            counter="128"
          >
          </v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('description')" rules="required|max:512">
          <v-textarea
            v-model="priorityModel.description"
            :error-messages="errors"
            :label="translator('description')"
            required
            outlined
            counter="512"
          >
          </v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />
        <v-btn @click="priorityCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import PriorityService from "@/services/PriorityService";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  data() {
    return {
      priorityModel: {},
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
    async priorityCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new PriorityService().create(this.priorityModel);
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
