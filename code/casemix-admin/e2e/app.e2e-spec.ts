import { CaseMixTemplatePage } from './app.po';

describe('CaseMix App', function() {
  let page: CaseMixTemplatePage;

  beforeEach(() => {
    page = new CaseMixTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
