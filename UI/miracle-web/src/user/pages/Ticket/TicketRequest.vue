<template>
  <v-container class="ticketColumn">
    <v-row style="justify-content: center; margin: 20px">
      <v-col cols="10">
        <v-icon>mdi-messages</v-icon>
        <h1 style="font-family: math">{{ translator("supportRequest") }}</h1>
      </v-col>
      <v-col cols="2" style="text-align-last: right">
        <v-btn color="success" dark class="mb-1" @click="showCreate">
          <v-icon class="mr-3"> mdi-plus </v-icon> {{ translator("createTicket") }}
        </v-btn>
      </v-col>
    </v-row>
    <v-divider />

    <v-row
      v-for="(n, index) in orderedTicketGroup"
      :key="index + n"
      class="newsRow elevation-3"
      style="justify-content: center"
    >
      <v-col cols="12" xl="8" md="8" sm="12">
        <v-layout class="d-md-block">
          <div class="text-justify">
            <div class="headline">
              <span style="font-size: large; color: black">
                <strong>{{ n.message }}</strong>
              </span>
              <div class="subtitle-2 date">
                <span style="font-size: smaller; color: black">
                  Title : <b>{{ n.title }}</b>
                </span>
              </div>
            </div>
          </div>
        </v-layout>
      </v-col>
      <v-col>
        <v-layout class="d-md-block" style="text-align-last: right">
          <div class="text-justify mt-3" style="display: inline !important">
            <b> {{ new Date(n.createdDate).toISOString().substr(0, 10).replace("T", " ") }} </b>
            {{ new Date(n.createdDate).toISOString().substr(10, 6).replace("T", " ") }}

            <v-icon class="pl-3" bold size="30px" v-model="n.isClosed">
              {{ n.isClosed === false ? "mdi-check-bold" : "mdi-close-circle" }}
            </v-icon>
            <v-icon md class="mr-2 reply ml-5" @click="ticketMessageDetails(n)">
              {{ n.IsRead === false ? "mdi-email-alert " : "mdi-email-open" }}
            </v-icon>
          </div>
        </v-layout>
      </v-col>
    </v-row>

    <v-dialog v-model="dialogEnable" :max-width="dialogWidth" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog"></component>
    </v-dialog>
  </v-container>
</template>

<script>
/*eslint-disable */
import TicketMessageCreate from "./components/TicketMessageCreate";
import TicketService from "@/services/TicketService";
import _ from "lodash";

export default {
  data() {
    return {
      ticketGroup: [],
      ticketGroupModel: {
        isRead: false,
      },
      ticketGroupId: 0,
      selectedTicket: {}, //Silme işlemi

      dialogEnable: false,
      dialogKey: 0,
      dialogWidth: 400,
      selectedComponent: "",
      componentProps: null,
      signalModel: {
        changes: false,
        returnValues: null,
      },
      pagedList: {},
      paginationModel: {
        page: 1,
        pageSize: -1,
      },

      loading: true,
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    async initialize() {
      this.loading = true;
      this.ticketGroup = [];
      let response = await new TicketService().getMessageGroups().finally(() => {
        this.loading = false;
      });
      this.ticketGroup = response.list;
    },
    showCreate() {
      this.selectedComponent = TicketMessageCreate;
      this.componentProps = null;

      this.openDialog(800);
    },
    async ticketMessageDetails(ticketGroup) {
      await new TicketService().getGroupReadInfo(ticketGroup.id);
      this.$router.push({
        path: "/user/ticket-details",
      });
      this.$store.commit("setTicketGroupId", ticketGroup.id);
    },
    openDialog(width = 400) {
      this.dialogEnable = true;
      this.dialogWidth = width;
    },
    closeDialog(signalModel) {
      const changes = signalModel.changes;
      const returnValues = signalModel.returnValues;
      if (changes) {
        this.setReturnValues(returnValues);
        this.initialize();
      }

      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    setReturnValues(values) {
      return values;
    },
  },
  computed: {
    orderedTicketGroup() {
      return _.orderBy(this.ticketGroup, ["createdDate"], ["desc"]);
    },
  },
};
</script>

<style scoped>
.ticketColumn {
  max-width: 60%;
}
.newsRow {
  background: #eeeeee;
}
</style>
