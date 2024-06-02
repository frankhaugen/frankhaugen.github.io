<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>QuestPDF</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>QuestPDF.Fluent</Namespace>
  <Namespace>QuestPDF.Helpers</Namespace>
  <Namespace>QuestPDF.Infrastructure</Namespace>
  <Namespace>QuestPDF.Previewer</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


await Document
	.Create(container =>
	{
		container.Page(page =>
		{
			page.Size(PageSizes.A4);
			page.Margin(2, Unit.Centimetre);
			page.PageColor(Colors.White);
			page.DefaultTextStyle(x => x.FontSize(20));

			page.Header()
				.Text("Hot Reload!")
				.SemiBold().FontSize(26).FontColor(Colors.Blue.Darken2);

			page.Content()
				.PaddingVertical(1, Unit.Centimetre)
				.Column(x =>
				{
					x.Item().Text("Hello, World!").FontSize(9).FontColor(Colors.Black);
				});

			page.Footer()
				.AlignCenter()
				.Text(x =>
				{
					x.Span("Page ");
					x.CurrentPageNumber();
				});
		});
	})
	.ShowInPreviewerAsync();