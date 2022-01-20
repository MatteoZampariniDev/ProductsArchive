import { OnInit } from "@angular/core";
import { AfterViewInit, Component, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Subject } from "rxjs";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { AuthorizeService } from "../../../api-authorization/authorize.service";
import { isNullOrWhiteSpaces } from "../helpers/string-extensions";
import { SortingDirection } from "../models/sorting-details";
import { IdentityService } from "../services/identity.service";

@Component({ template: '' })
export abstract class TableViewBaseComponent<TItem>
  implements OnInit, AfterViewInit {

  constructor(protected formBuilder: FormBuilder,
    protected authService: AuthorizeService,
    protected identityService: IdentityService) { }

  //#region HTML
  public isAuthenticated: boolean = false;
  public role: string = '';

  public title: string;
  public description: string;
  public itemEditorRoute: string;
  public tableHeaders: string[];
  public tableHeadersIds: string[];
  public defaultSortHeaderId: string;
  public defaultSortDirection: string = SortingDirection.ascending;

  public filterForm: FormGroup;
  public tableData: MatTableDataSource<TItem>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  public onFilterQueryChanged(): void {
    if (this._filterQueryDebounceTracker.observers.length === 0) {
      this._filterQueryDebounceTracker
        .pipe(debounceTime(800), distinctUntilChanged())
        .subscribe(() => this.onFilterOrSortChanged()
        );
    }

    this._filterQueryDebounceTracker.next(this.filterQuery());
  }

  public abstract onFilterOrSortChanged(): void;

  public abstract onPageChanged(event: PageEvent): void;

  public abstract onDeleteItemRequest(item: TItem): void;

  public hasRole(requiredRole: string): boolean {
    return this.isAuthenticated && this.role === requiredRole;
  }
  //#endregion

  //#region Internal
  protected filterQuery(): string { return this.filterForm.get('filterQuery').value; }

  private _filterQueryDebounceTracker: Subject<string> = new Subject<string>();

  ngOnInit(): void {
    this.authService.isAuthenticated()
      .subscribe(
        (v) => {
          this.isAuthenticated = v;

          if (this.isAuthenticated) {

            this.identityService.getUserId((userId) => {

              if (!isNullOrWhiteSpaces(userId)) {

                this.identityService.getRole(userId, (role) => {

                  this.role = role;
                })
              }
            })
          }
        });
  }

  ngAfterViewInit(): void {
    this.updatePage();
  }

  /** Call in the constructor or in ngOnInit to configure the component */
  protected initializeComponent(title: string, description: string, itemEditorRoute: string, tableHeaders: string[], tableHeadersIds: string[]) {
    this.title = title;
    this.description = description;
    this.itemEditorRoute = itemEditorRoute;
    this.tableHeaders = tableHeaders;
    this.tableHeadersIds = tableHeadersIds;
    this.defaultSortHeaderId = this.tableHeadersIds[0];

    this.filterForm = this.formBuilder.group({
      filterQuery: new FormControl('')
    });
  }

  protected updatePage(): void {
    this.updatePageWithEvent(null);
  }

  protected abstract updatePageWithEvent(event: PageEvent): void;
  //#endregion
}
