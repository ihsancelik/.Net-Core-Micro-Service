<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("editPriority") }}</h2>
      <hr class="mb-6" />

      <v-form>
        <ValidationProvider #default="{ errors }" :name="translator('name')" rules="required|max:128">
          <v-text-field
            v-model="priorityModel.name"
            :label="translator('name')"
            :error-messages="errors"
            required
            outlined
            counter="128"
          >
          </v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('state')" rules="required|max:128">
          <v-text-field
            v-model="priorityModel.state"
            :label="translator('state')"
            :error-messages="errors"
            required
            outlined
            counter="128"
          >
          </v-text-field>
        </ValidationProvider>

        <ValidationProvider #default="{ errors }" :name="translator('description')" rules="required|max:512">
          <v-textarea
            v-model="priorityModel.description"
            :label="translator('description')"
            :error-messages="errors"
            required
            outlined
            counter="512"
          >
          </v-textarea>
        </ValidationProvider>

        <hr class="mb-2" />

        <v-btn @click.prevent="priorityEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
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
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["priorityIdProp"],
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
    async initialize() {
      let response = await new PriorityService().getById(this.priorityId);
      if (response === ReturnConstraints.REFRESH) await this.initialize();
      this.priorityModel.id = this.priorityId;
      this.priorityModel = response.data;
    },
    async priorityEdit() {
      let response = await new PriorityService().edit(this.priorityModel, this.priorityId);
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
    this.priorityId = this.priorityIdProp;
    this.initialize();
  },
};
</script>

<style scoped></style>
