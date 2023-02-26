<template>
  <ValidationObserver ref="observer">
    <v-container style="width: 1200px; display: grid">
      <v-card class="pa-10 pt-5" style="display:inline; !important" elevation="5">
        <a @click="backTo()" large>
          <v-icon style="float: left"> mdi-keyboard-backspace </v-icon>
        </a>

        <h2 class="text-md-center">{{ translator("ticketMessageDetails") }}</h2>
        <hr class="mb-3 mt-3" />

        <v-row v-for="item in orderedTicketGroups" :key="item.id" style="display: block !important">
          <!-- Admin -->
          <v-col cols="12" v-if="item.isAdmin" class="adminReply mt-5" style="float: right; !important">
            <div>
              <v-icon style="float: left" class="mr-2"> mdi-account-edit </v-icon> <b> {{ adminName }} </b>
              <p style="float: right">
                <b> {{ new Date(item.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
                {{ new Date(item.createdDate).toISOString().substr(10, 6).replace("T", " ") }}
                <v-icon v-if="item.imageName != null" @click="openImage(item.id)" class="mr-2">
                  mdi-image-filter-hdr
                </v-icon>
              </p>
            </div>
            <div class="mt-5">
              <p class="mt-3">{{ item.message }}</p>
            </div>
            <v-img :src="item.ImageName" width="100%"></v-img>
          </v-col>

          <!-- Customer -->
          <v-col cols="12" v-else class="customer mt-5" style="float: left; !important">
            <div>
              <v-icon style="float: left" class="mr-2"> mdi-account </v-icon>
              <b> {{ item.userName }} </b>
              <p style="float: right">
                <b> {{ new Date(item.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
                {{ new Date(item.createdDate).toISOString().substr(10, 6).replace("T", " ") }}
                <v-icon v-if="item.imageName != null" @click="openImage(item.id)" class="mr-2">
                  mdi-image-filter-hdr
                </v-icon>
              </p>
            </div>
            <div style="margin-top: 2%">
              <p class="mt-2">{{ item.message }}</p>
            </div>
          </v-col>
        </v-row>

        <v-btn v-if="this.isCanceled" large outlined color="black" width="100%" class="answer" @click="showReply">
          {{ translator("answer") }}
        </v-btn>
      </v-card>
    </v-container>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
/* eslint-disable */
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import TicketMessageReply from "./TicketMessageReply";
import OpenImage from "./OpenImage";
import TicketService from "@/services/TicketService";
import _ from "lodash";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  data() {
    return {
      ticketGroups: [],
      groupId: 0,
      userId: 0,
      role: "",
      isCanceled: true,

      adminName: "",

      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: null,
      selectedComponent: "",
      componentProps: null,
    };
  },
  created() {
    this.groupId = this.$route.params.groupId;
    this.userId = this.$route.params.userId;
    this.initialize();
  },
  methods: {
    async initialize() {
      this.ticketGroups = [];

      let isCancel = await new TicketService().getGroupCloseInfo(this.groupId);
      this.isCanceled = isCancel.success;

      let ticketMessageResponse = await new TicketService().getTicketMessages(this.groupId);
      let userInfoResponse = await new TicketService().getUserInfo(this.userId);

      for (const message of ticketMessageResponse.list) {
        message.company = userInfoResponse.data.company;
        message.userName = userInfoResponse.data.userName;
        this.ticketGroups.push(message);
        if (message.adminName != "") this.adminName = message.adminName;
        if (message.adminName == "") this.adminName;
      }
    },
    showReply() {
      this.selectedComponent = TicketMessageReply;
      this.componentProps = {
        groupIdProps: this.groupId,
      };
      this.openDialog(600);
    },
    openImage(id) {
      this.selectedComponent = OpenImage;
      this.componentProps = {
        messageIdProp: id,
      };
      this.openDialog(1000);
    },
    backTo() {
      this.$router.push({
        path: "/admin/ticket-res",
      });
    },
    openDialog(width = 500) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
      this.initialize();
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  computed: {
    orderedTicketGroups() {
      return _.orderBy(this.ticketGroups, ["createdDate"], ["asc"]);
    },
  },
};
</script>

<style scoped>
.customer {
  font-family: "" Roboto ",sans-serif !important";
  font-size: 16px;
  letter-spacing: -0.3px;
  border-radius: 10px 10px 10px 0px;
  background-color: rgb(231, 241, 174);
  padding: 2%;
  max-width: 70%;
  float: left;
}
.adminReply {
  border-radius: 10px 10px 0px 10px;
  font-family: "" Roboto ",sans-serif !important";
  font-size: 16px;
  max-width: 70%;
  margin-bottom: 20px;
  padding: 2%;
  background-color: rgb(195, 227, 240);
  float: right;
}
.answer {
  margin-top: 3%;
  padding: initial !important;
  position: relative;
}
</style>
