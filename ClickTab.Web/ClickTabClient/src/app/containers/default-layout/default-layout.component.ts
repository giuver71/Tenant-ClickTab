import { Component, TemplateRef, ViewChild } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { ActivatedRoute, NavigationEnd, Router } from "@angular/router";
import * as signalR from "@microsoft/signalr";
import { Subscription } from "rxjs";
import { environment } from "./../../../environments/environment";
import { UserDTO } from "./../../models/generics/user.model";
import { NotificationDetailDTO } from "./../../models/notification-center/notificationDetail.model";
import { AuthService } from "./../../services/auth.service";
import { DialogService } from "./../../services/dialog.service";
import { EventHandlerService } from "./../../services/eventHandler.service";
import { NotificationService } from "./../../services/notification.service";

import { MatMenuTrigger } from "@angular/material/menu";
import { navItems } from "./_nav";
import { MenuService } from "../../services/generics/menu.service";
import { RoleDTO } from "../../models/generics/role.model";
import { RoleService } from "../../services/generics/role.services";
import { INavData } from "@eqproject/eqp-ui";
import { RoleRuleDTO } from "../../models/generics/rolerule.model";
import { UserService } from "../../services/user.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html"
})
export class DefaultLayoutComponent {
  public navItems! :any;
  currentUser!: UserDTO;
  enableNotificationSystem: boolean = environment.enableNotificationSystem;
  notificationCount: number = 0;
  notificationList: Array<NotificationDetailDTO> = new Array<NotificationDetailDTO>();
  notificationReadSubscription!: Subscription;
  socketReloadCounter: number = 0;
  manageableRoles:Array<RoleDTO>=new Array<RoleDTO>();
  hashedRoles: Array<string> = new Array<string>();
  currentRole:RoleDTO=new RoleDTO();
  loaded:boolean=false;
  private hubConnection!: signalR.HubConnection;
  @ViewChild(MatMenuTrigger) trigger!: MatMenuTrigger;

  // Dialog per la visualizzazione di una notifica
  dialogViewNotificationRef!: MatDialogRef<TemplateRef<any>>;
  @ViewChild("dialogViewNotification", { static: false }) dialogViewNotification!: TemplateRef<any>;
  selectedNotification!: NotificationDetailDTO;
  
  public perfectScrollbarConfig = {
    suppressScrollX: true
  };

  navitems!:INavData[];

  constructor(
    private menuService:MenuService,
    private authService: AuthService,
    private eventHandlerService: EventHandlerService,
    private router: Router,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private activatedRoute: ActivatedRoute,
    private roleService:RoleService,
    private userService:UserService
  ) {
    this.resetRouteReuseStrategy();
  }

  /**
   * Reimposta la strategia per il reload delle rotte al valore di default.
   * Questo metodo viene richiamato ogni volta che si inizializza il componente, cioè in due momenti: quando viene avviata l'app dopo la login e quando si
   * cambia la company selezionata (in quest'ultimo viene forzato il reload della rotta corrente per potere aggiornare i dati).
   */
  resetRouteReuseStrategy() {
    this.router.routeReuseStrategy.shouldReuseRoute = (future, curr) => {
      return future.routeConfig === curr.routeConfig;
    };
    this.router.onSameUrlNavigation = "ignore";
  }

 async  ngOnInit() {

  

  // this.navItems=navItems;
    this.currentUser=this.authService.getCurrentUser();
    this.currentRole=this.authService.getCurrentRole();
    //Se si vuole avviare il sistema di notifiche ed è presente l'utente autenticato
    //allora avvia i servizi per la ricezione delle notifiche push
    if (
      this.enableNotificationSystem &&
      this.authService.getCurrentUser() != null &&
      this.authService.getCurrentUser() != undefined
    ) {
      //TODO: Decommentare a fine refactoring
      // this.initializeSocketNotificationConnection();
    }
    await this.getRoles();
    this.getMenu();
  }

  

   async getRoles(){
    this.loaded=false;
       await this.userService.getAllRolesUserFacilityHashed(this.currentUser.ID).then((hashedRoles) => {
      this.hashedRoles = hashedRoles;
      // Check del Ruolo
      var roleRules: Array<RoleRuleDTO> = new Array<RoleRuleDTO>();
      // res = res.filter(x => x.RoleDTO.FK_Facilitie == this.currentFacilitieId || x.RoleDTO.FK_Facilitie == null);
          this.hashedRoles.forEach((role) => {
            let decodedrole = this.authService.decodeToken(role);
            //todo tagliare l'oggetto user
            decodedrole.Role.RoleRules.forEach((roleRule) => {
              roleRules.push(roleRule);
            });
            this.manageableRoles.push(decodedrole.Role);
          });

          // this.currentRoleTypes = this.authService.getCurrentRoleTypes(roleRules);
          this.currentRole = this.authService.getCurrentRole();

          // se non trova il currentRole lo imposta
          if (this.currentRole == null && this.manageableRoles.length > 0) {
            this.currentRole = this.manageableRoles[0];
            this.authService.setCurrentRole(this.hashedRoles[0]);
          } else if (this.manageableRoles.length > 0) {
            let roleIndex = this.manageableRoles.findIndex((x) => x.ID == this.currentRole.ID);
            this.currentRole = this.manageableRoles[roleIndex];
            this.authService.setCurrentRole(this.hashedRoles[roleIndex]);
          }
          this.loaded=true;
      });   
  }
 
  onSelectedRole(ev:RoleDTO){
      this.router.navigate(["/dashboard"]).then(() => {
          this.currentRole=ev;
          let roleiindex = this.manageableRoles.findIndex((x) => x.ID == ev.ID);
          this.authService.setCurrentRole(this.hashedRoles[roleiindex]);
          this.reloadComponent();
      });
  }

     reloadComponent() {
        let dashBoardUrl: string = "/dashboard";
        if (window.innerWidth >= 992) {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.onSameUrlNavigation = "reload";
          this.router.navigate([dashBoardUrl], { relativeTo: this.activatedRoute });
        } else {
          location.reload();
        }
  }

  getMenu(){
    let currentRole:RoleDTO=this.authService.getCurrentRole();
    this.menuService.getMenuByRole(this.currentRole.ID).then((res=>{
        this.navItems=res;
    })).catch((err)=>{
        DialogService.Error(err.message);
        console.error("default-layout.getMenu",err);

    })
  }


  //#region Gestione Hub notifiche SignalR

  async initializeSocketNotificationConnection() {
    await this.startConnection();
    this.loadUnreadNotification(false);
    this.notificationReadSubscription = this.eventHandlerService.subscribe("NotificationReadEvent", (response) => {
      this.notificationCount = this.notificationCount > 0 ? this.notificationCount - 1 : 0;
    });
  }

  /**
   * Costruisce una connessione con l'hub che gestisce il web socket per l'invio delle notifiche in tempo reale
   * e la avvia.
   */
  public async startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiUrl + "/NotificationHub", {
        // skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection.onclose((error: Error) => {
      if (error) {
        console.error(error);
        this.retrySocketConnection();
      }
    });

    this.NewConnection();
    this.NotificationDataUpdated();

    await this.hubConnection
      .start()
      .then(() => {
        this.onConnection();
      })
      .catch((err) => {
        console.log("Error while starting connection: " + err);
        this.retrySocketConnection();
      });

    if (this.hubConnection.state == signalR.HubConnectionState.Connected) this.socketReloadCounter = 0;
  }

  /**
   * Metodo per tentate di riconnettersi al socket delle notifiche quando la connessione va giù.
   * Viene fatto un tentativo ogni 5 secondi per un massimo di 5 volte.
   */
  retrySocketConnection() {
    if (this.socketReloadCounter < 5) {
      setTimeout(async () => {
        this.socketReloadCounter = this.socketReloadCounter + 1;
        console.log(
          "Attempt to re-establish connection with the notification socket number " + this.socketReloadCounter + "..."
        );

        await this.startConnection();

        if (this.socketReloadCounter == 5) {
          console.log("Connection to notification socket failed. Reload the page to try Again.");
          DialogService.Warning(
            "Connessione al sistema di notifiche non riuscita. Ricaricare la pagina per riprovare.",
            "Attenzione!"
          );
        }
      }, 5000);
    }
  }

  /**
   * Una volta aperta la connessione invoca il metodo "Connect" dell'hub per registrare la connessione dell'utente loggato
   */
  public onConnection() {
    //In caso di entità con ID stringa modificare il metodo "Connect" nella classe NotificationHub.cs all'interno del progetto web.
    this.hubConnection.invoke("Connect", this.currentUser.ID);
  }

  public NewConnection() {
    this.hubConnection.on("MyConnectionId", (connectionID) => {
      console.log("Successfully connected to notification hub.");
    });
  }

  /**
   * Metodo chiamato quando l'hub notifica all'utente loggato la ricezione di una nuova notifica.
   * Aumenta il contatore di 1, mostra il toast con il titolo della notifica ricevuta e emette l'evento "NotificationUpdateEvent".
   */
  public NotificationDataUpdated() {
    this.hubConnection.on("NotificationDataUpdate", async (notificationMessage) => {
      this.notificationCount = this.notificationCount > 0 ? this.notificationCount + 1 : 1;
      await this.loadUnreadNotification(false);
      DialogService.ShowNewNotification(notificationMessage, null);
      this.eventHandlerService.broadcast({ name: "NotificationUpdateEvent", content: "" });
    });
  }

  /**
   * Carica le notifiche non lette dell'utente loggato.
   * Se viene passato il parametro a true apre la tendina delle notifiche.
   * @param openDropdown
   */
  async loadUnreadNotification(openDropdown: boolean = true) {
    this.notificationList = new Array<NotificationDetailDTO>();
    await this.notificationService
      .getNotifications(true)
      .then((res) => {
        this.notificationCount = res && res.length > 0 ? res.length : 0;
        this.notificationList = res;
        // Se le notifiche hanno bisogno di essere tradotte o vanno sostituiti i placeholder
        // bisogna usare la riga qui sotto. Il template di default esegue queste operazioni lato server
        // quando le notifiche vengono recuperate.
        // this.notificationList = res ? this.notificationService.replaceNotificationsPlaceholders(res) : new Array<NotificationDetailDTO>();
        if (openDropdown) this.trigger.openMenu();
      })
      .catch((err) => {
        DialogService.Error(err.message);
      });
  }

  /**
   * Se viene aperta una notifica non letta salva data/ora di lettura e apre il dialog per la visualizzazione della stessa.
   * @param notificationDetail
   */
  readNotification(notificationDetail: NotificationDetailDTO) {
    if (!notificationDetail.ReadDate) {
      this.notificationService
        .markAsRead(notificationDetail.ID)
        .then((res) => {
          notificationDetail.ReadDate = res;
          this.eventHandlerService.broadcast({
            name: "NotificationReadEvent",
            content: { ReadDate: res, ID: notificationDetail.ID }
          });
        })
        .catch((err) => {
          DialogService.Error(err.message);
        });
    }
    this.selectedNotification = notificationDetail;
    this.dialogViewNotificationRef = this.dialog.open(this.dialogViewNotification, {
      disableClose: false,
      hasBackdrop: true,
      autoFocus: false,
      width: "40%"
    });
  }

  /**
   * Redirect alla lista delle notifiche ricevute dall'utente.
   */
  goToNotificationList() {
    this.router.navigate(["/list-notifications"], { relativeTo: this.activatedRoute });
  }

  //#endregion

  
}
