<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("editNotice") }}</h2>
      <hr class="mb-3" />

      <v-form>
        <v-row>
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('text')" rules="required">
              <v-textarea
                :style="fieldStyle"
                v-model="noticeModel.text"
                :error-messages="errors"
                :label="translator('text')"
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
                  required
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="noticeModel.startDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="startDateModal = false">{{ translator("cancel") }}</v-btn>
                <v-btn text color="primary" @click="$refs.startDateDialog.save(noticeModel.startDate)"
                  >{{ translator("ok") }}
                </v-btn>
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
                  required
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="noticeModel.endDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="endDateModal = false">{{ translator("cancel") }}</v-btn>
                <v-btn text color="primary" @click="$refs.endDateDialog.save(noticeModel.endDate)">{{
                  translator("ok")
                }}</v-btn>
              </v-date-picker>
            </v-dialog>
          </v-col>
        </v-row>

        <hr class="mb-2" />
        <v-btn @click.prevent="noticeEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
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
import NoticeService from "@/services/NoticeService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });
extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["noticeIdProp"],
  data() {
    return {
      fieldStyle: "margin-bottom: -3%;margin-top: -3%;",
      noticeModel: {
        startDate: new Date().toISOString().substr(0, 10),
        endDate: new Date().toISOString().substr(0, 10),
      },
      endDateModal: false,
      startDateModal: false,
      noticeId: 0,

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
      let response = await new NoticeService().getById(this.noticeId);
      if (response === ReturnConstraints.REFRESH) await this.initialize();

      this.noticeModel.id = this.noticeId;
      this.noticeModel = response.data;
      this.noticeModel.startDate = response.data.startDate.toString().substr(0, 10);
      this.noticeModel.endDate = response.data.endDate.toString().substr(0, 10);
    },
    async noticeEdit() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let response = await new NoticeService().edit(this.noticeModel, this.noticeId);
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
    this.noticeId = this.noticeIdProp;
    this.initialize();
  },
};
</script>
