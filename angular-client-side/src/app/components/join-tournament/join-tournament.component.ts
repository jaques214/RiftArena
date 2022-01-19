import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '@models/user';
import { Team } from '@models/team';
import { Tournament } from '@models/tournament';
import { UserRestService } from '@services/user-rest/user-rest.service';
import { TeamRestService } from '@services/team-rest/team-rest.service';
import { TourneyRestService } from '@services/tourney-rest/tourney-rest.service';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import ConfirmedValidator from '@src/app/confirmed.validator';
import { MatTableDataSource } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';

export interface PeriodicElement {
  name: string;
}

@Component({
  selector: 'app-join-tournament',
  templateUrl: './join-tournament.component.html',
  styleUrls: ['./join-tournament.component.css']
})
export class JoinTournamentComponent implements OnInit {
  tourney!: Tournament;
  teams: any = [];
  userNickname!:string;
  user!:User;
  form: FormGroup = new FormGroup({
    tourneys: new FormControl(''),
  });
  isShow = true;
  tournaments: any = [];
  tourneysList: any = [];
  idList: any = [];
  ELEMENT_DATA: PeriodicElement[] = [];
  dataSource: any;
  displayedColumns: string[] = ['select', 'name'];
  selection = new SelectionModel<PeriodicElement>(true, []);
  

  constructor(private tournamentRestService: TourneyRestService,
     private teamRestService: TeamRestService,
     private userRestService: UserRestService,
     private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.getUser().subscribe((user) => {
      this.userNickname = user.nickname!;
    });

    this.getTeams().subscribe((data: {}) => {
        this.teams = data;
        console.log(this.teams);
        this.populateTable();
        console.log(this.ELEMENT_DATA);
        this.dataSource = new MatTableDataSource<PeriodicElement>(this.ELEMENT_DATA);
    });

    this.getTournaments().subscribe((data: {}) => {
      this.tournaments = data;
      this.populateTourneys();
      console.log(this.tourneysList);
      console.log(this.idList);

      this.form = this.formBuilder.group({
        tourneys: ['', Validators.required],
      },
      {
        validators: [ConfirmedValidator.matchUser('tourneys', this.tourneysList)]
      });
    });

  }

  populateTable() {
    const tam = this.teams.length;
    console.log(tam);
    for (let index = 0; index < tam; index++) {
      this.ELEMENT_DATA[index] = {
        name: this.teams[index].name, 
      };
    }
    console.log(this.ELEMENT_DATA);
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource?.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: PeriodicElement): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.name + 1}`;
  }

  populateTourneys() {
    this.tournaments.forEach((element:any) => {
      this.tourneysList.push(element.name);
      this.idList.push(element.tournamentId);
    });
  }

  selectTourney(value: number) {
    this.isShow = !this.isShow;
    const index = this.tourneysList.indexOf(value);
    const id = this.idList[index];
    
    this.getTournament(id).subscribe(tourney => {
      this.tourney = tourney;
 });
  }

  getErrorMessage() {
    if (this.form.get('tourneys')?.hasError('required')) {
      return 'You must enter a value';
    }

    return (this.form.get('tourneys')?.hasError('tourneys') || this.form.get('tourneys')?.errors?.['matching']) ? "This tournament doesn't exist." : "";
  }

  getTournaments() {
    return this.tournamentRestService.getTourneys();
  }

  getTournament(id: number) {
    return this.tournamentRestService.getTourney(id);
  }

  getTeams(): Observable<Team[]> {
    return this.teamRestService.getTeams();
  }

  getUser() {
    return this.userRestService.getUser();
  }

  addTeam(id: number, nickname: string) {
    this.selection.selected[0]?.name
    this.tournamentRestService.addTeam(id, nickname).subscribe({
      next: () => {
        this.getUser().subscribe((user) => {
          this.userNickname = user.nickname!;
        });
      },
      error: (err) => console.log(err)
    });
  }
}
