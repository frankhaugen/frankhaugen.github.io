# frankhaugen.github.io

Source for [https://frankhaugen.github.io/](https://frankhaugen.github.io/): custom **Jekyll** site (dark sci-fi shell), project cards from [`_data/projects.yml`](_data/projects.yml), and posts under [`_posts/`](_posts/).

## Blogging (markdown only)

1. Add `_posts/YYYY-MM-DD-your-slug.md`.
2. Start with YAML front matter, for example:

```yaml
---
title: Your title
tags: [dotnet, opinion]
description: One-line summary for listings and SEO.
---

Your markdown body…
```

3. Push to the default branch (`master` for this repo). GitHub Pages runs Jekyll; the post appears on `/blog/` and at `/blog/:year/:month/:day/:title/`.

RSS: `/feed.xml` (via `jekyll-feed`). Meta tags via `jekyll-seo-tag`.

### Citations (Kramdown)

Use **footnotes** for inline attribution and a **`## References`** section so skimmers still see sources:

```markdown
RFC 3339 profiles ISO 8601 for timestamps on the wire.[^rfc3339]

[^rfc3339]: https://datatracker.ietf.org/doc/html/rfc3339 — RFC 3339 (IETF).

## References

- [RFC 3339 — Date and Time on the Internet](https://datatracker.ietf.org/doc/html/rfc3339)
```

Footnotes render at the bottom of the post body; theme styles live in `assets/css/theme.css`.

## Archived assets

YouTube episode trees, LinqPad dumps, and other bulk files stay in [`frankhaugen/frankhaugen` → `unsorted/from-github-pages`](https://github.com/frankhaugen/frankhaugen/tree/main/unsorted/from-github-pages).

## Local preview

Install Ruby + Jekyll ([docs](https://jekyllrb.com/docs/)), then:

```bash
bundle install   # optional if you add a Gemfile
jekyll serve
```
