import { Component } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";


@Component({
  template: ''
})
export abstract class EditorBaseComponent {
  constructor(protected formBuilder: FormBuilder,
    protected activatedRoute: ActivatedRoute,
    protected router: Router) {

  }

  //#region HTML
  public form: FormGroup;
  public id?: string;
  public title: string;
  public description: string;
  public formExitRoute;

  public onSubmit(): void {
    if (this.id) {
      this.updateItem();
    }
    else {
      this.createItem();
    }
  }

  public compareById(obj1: any, obj2: any): boolean {
    return obj1.id === obj2.id;
  }
  //#endregion

  //#region Internal
  /** Call on ngOnInit before initializeComponent() */
  protected getRouteParams(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
  }

  /** Call on ngOnInit to configure the component */
  protected initializeComponent(title: string, description: string, formExitRoute: string, formControls: { [key: string]: any }): void {
    this.form = this.formBuilder.group(formControls);
    this.title = title;
    this.description = description;
    this.formExitRoute = formExitRoute;
  }

  protected exitForm(): void {
    this.router.navigate([this.formExitRoute]);
  }

  protected abstract createItem(): void;
  protected abstract updateItem(): void;
  //#endregion
}



    // HELPER METHODS

    //// retrieve a FormControl
    //getControl(name: string)
    //{
    //    return this.form.get(name);
    //}

    //// true if the FormControl is valid
    //isValid(name: string)
    //{
    //    var e = this.getControl(name);
    //    return e && e.valid;
    //}

    //// true if the FormControl has been changed
    //isChanged(name: string)
    //{
    //    var e = this.getControl(name);
    //    return e && (e.dirty || e.touched);
    //}

    //// true if the FormControl is raising an error
    //hasError(name: string)
    //{
    //    var e = this.getControl(name);
    //    return e && (e.dirty || e.touched) && e.invalid;
    //}
