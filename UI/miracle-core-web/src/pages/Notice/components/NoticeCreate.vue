<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newNotice") }}</h2>
      <hr />

      <v-form>
        <v-row>
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('text')" rules="required">
              <v-textarea
                v-model="noticeModel.text"
                :label="translator('text')"
                :error-messages="errors"
                :style="fieldStyle"
                required
                outlined
              ></v-textarea>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="12" md="6">
            <v-dialog
              ref="startDateDialog"
              v-model="startDateModal"
              :return-value.sync="noticeModel.startDate"
              persistent
              width="290px"
            >
              <template #activator="{ on }">
                <v-text-field
                  v-model="noticeModel.startDate"
                  :label="translator('startDate')"
                  v-on="on"
                  readonly
                  required
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="noticeModel.startDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="startDateModal = false">Cancel</v-btn>
                <v-btn text color="primary" @click="$refs.startDateDialog.save(noticeModel.startDate)">OK </v-btn>
              </v-date-picker>
            </v-dialog>
          </v-col>
          <v-col cols="12" md="6">
            <v-dialog
              ref="endDateDialog"
              v-model="endDateModal"
              :return-value.sync="noticeModel.endDate"
              persistent
              width="290px"
            >
              <template #activator="{ on }">
                <v-text-field
                  v-model="noticeModel.endDate"
                  :label="translator('endDate')"
                  v-on="on"
                  readonly
                  required
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="noticeModel.endDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="endDateModal = false">Cancel</v-btn>
                <v-btn text color="primary" @click="$refs.endDateDialog.save(noticeModel.endDate)">OK</v-btn>
              </v-date-picker>
            </v-dialog>
          </v-col>
        </v-row>

        <hr class="mb-2" />

        <v-btn @click.prevent="noticeCreate" color="primary" dark class="v-btn--block v-size--large mb-2">{{
          translator("create")
        }}</v-btn>
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
import NoticeService from "@/services/NoticeService";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  data() {
    return {
      fieldStyle: "margin-bottom: -3%;margin-top: 3%;",
      startDateModal: false,
      endDateModal: false,

      noticeModel: {
        startDate: new Date().toISOString().substr(0, 10),
        endDate: new Date().toISOString().substr(0, 10),
      },

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
    async noticeCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new NoticeService().create(this.noticeModel);
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
  created() {
    this.noticeModel.isActive = false;
  },
};
</script>
